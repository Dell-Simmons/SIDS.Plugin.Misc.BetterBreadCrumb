using Nop.Core;
using Nop.Core.Caching;
using Nop.Core.Domain.Catalog;
using Nop.Core.Domain.Customers;
using Nop.Core.Domain.Media;
using Nop.Core.Domain.Orders;
using Nop.Core.Domain.Security;
using Nop.Core.Domain.Seo;
using Nop.Core.Domain.Shipping;
using Nop.Core.Domain.Vendors;
using Nop.Services.Catalog;
using Nop.Services.Common;
using Nop.Services.Customers;
using Nop.Services.Directory;
using Nop.Services.Helpers;
using Nop.Services.Localization;
using Nop.Services.Media;
using Nop.Services.Orders;
using Nop.Services.Security;
using Nop.Services.Seo;
using Nop.Services.Shipping.Date;
using Nop.Services.Stores;
using Nop.Services.Tax;
using Nop.Services.Vendors;
using Nop.Web.Factories;
using Nop.Web.Models.Catalog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SIDS.Plugin.Misc.BetterBreadCrumb.Factories
{
    /// <summary>
    /// Represents the product model factory
    /// </summary>
    public class BetterProductModelFactory : ProductModelFactory
    { 
        #region Fields
        private readonly CatalogSettings _catalogSettings;
        private readonly ICategoryService _categoryService;
        private readonly IGenericAttributeService _genericAttributeService;
        private readonly ILocalizationService _localizationService;
        private readonly IUrlRecordService _urlRecordService;
        private readonly IWorkContext _workContext;
        #endregion Fields

        #region Constructors
        public BetterProductModelFactory(
            CaptchaSettings captchaSettings,
            CatalogSettings catalogSettings,
            CustomerSettings customerSettings,
            ICategoryService categoryService,
            ICurrencyService currencyService,
            ICustomerService customerService,
            IDateRangeService dateRangeService,
            IDateTimeHelper dateTimeHelper,
            IDownloadService downloadService,
            IGenericAttributeService genericAttributeService,
            ILocalizationService localizationService,
            IManufacturerService manufacturerService,
            IPermissionService permissionService,
            IPictureService pictureService,
            IPriceCalculationService priceCalculationService,
            IPriceFormatter priceFormatter,
            IProductAttributeParser productAttributeParser,
            IProductAttributeService productAttributeService,
            IProductService productService,
            IProductTagService productTagService,
            IProductTemplateService productTemplateService,
            IReviewTypeService reviewTypeService,
            IShoppingCartService shoppingCartService,
            ISpecificationAttributeService specificationAttributeService,
            IStaticCacheManager staticCacheManager,
            IStoreContext storeContext,
            IStoreService storeService,
            IShoppingCartModelFactory shoppingCartModelFactory,
            ITaxService taxService,
            IUrlRecordService urlRecordService,
            IVendorService vendorService,
            IWebHelper webHelper,
            IWorkContext workContext,
            MediaSettings mediaSettings,
            OrderSettings orderSettings,
            SeoSettings seoSettings,
            ShippingSettings shippingSettings,
            VendorSettings vendorSettings,
            IVideoService videoService) : base(
            captchaSettings,
            catalogSettings,
            customerSettings,
            categoryService,
            currencyService,
            customerService,
            dateRangeService,
            dateTimeHelper,
            downloadService,
            genericAttributeService,
            localizationService,
            manufacturerService,
            permissionService,
            pictureService,
            priceCalculationService,
            priceFormatter,
            productAttributeParser,
            productAttributeService,
            productService,
            productTagService,
            productTemplateService,
            reviewTypeService,
            shoppingCartService,
            specificationAttributeService,
            staticCacheManager,
            storeContext,
            storeService,
            shoppingCartModelFactory,
            taxService,
            urlRecordService,
            vendorService,
            videoService,
            webHelper,
            workContext,
            mediaSettings,
            orderSettings,
            seoSettings,
            shippingSettings,
            vendorSettings)
        {
            _catalogSettings = catalogSettings;
            _categoryService = categoryService;
            _genericAttributeService = genericAttributeService;
            _localizationService = localizationService;
            _urlRecordService = urlRecordService;
            _workContext = workContext;
        }
        #endregion

        #region Methods

        //
        // Only used for BetterBreadCrumb
        // Parses referringUrl and returns the "parent" of the page we are heading for
        //
        private async Task<Category> GetReferringCategoryAsync(string referringUrl, IList<ProductCategory> categories)
        {
            var referringUrlSplit = referringUrl.Split('/');
            if (referringUrlSplit.Length > 0)
            {
                for (var i = 0; i < referringUrlSplit.Length; i++)
                {
                    if (referringUrlSplit[i].Length > 0)
                    {
                        var referringCategoryURL = referringUrlSplit[i];
                        var urlRecordService = Nop.Core.Infrastructure.EngineContext.Current
                            .Resolve<IUrlRecordService>();
                        var url = await urlRecordService.GetBySlugAsync(referringCategoryURL);
                        if (url != null)
                        {
                            if (string.Equals(url.EntityName, "category", StringComparison.OrdinalIgnoreCase))
                            {
                                var referringCategory = await _categoryService.GetCategoryByIdAsync(url.EntityId);//.FirstOrDefault();
                                if (referringCategory != null)
                                {
                                    return await _categoryService.GetCategoryByIdAsync(referringCategory.Id);
                                }
                                return null;
                            }
                        }
                    }
                }
            }
            return await _categoryService.GetCategoryByIdAsync(categories[0].CategoryId);
        }
       

        protected override async Task<ProductDetailsModel.ProductBreadcrumbModel> PrepareProductBreadcrumbModelAsync(
            Product product)
        {
            if (product == null)
            {
                throw new ArgumentNullException(nameof(product));
            }

            var breadcrumbModel = new ProductDetailsModel.ProductBreadcrumbModel
            {
                Enabled = _catalogSettings.CategoryBreadcrumbEnabled,
                ProductId = product.Id,
                ProductName = await _localizationService.GetLocalizedAsync(product, x => x.Name),
                ProductSeName = await _urlRecordService.GetSeNameAsync(product)
            };
            var productCategories = await _categoryService.GetProductCategoriesByProductIdAsync(product.Id);
            if (!productCategories.Any())
            {
                return breadcrumbModel;
            }

            var category = await _categoryService.GetCategoryByIdAsync(productCategories[0].CategoryId);

            //+++++++++++++++++++++++++++++++
            // Heart of the BetterBreadcrumb Plugin
            //+++++++++++++++++++++++++++++++

            var previousPageUrl = await _genericAttributeService.GetAttributeAsync<string>(
                await _workContext.GetCurrentCustomerAsync(),
                "BetterBreadCrumbURL");

            if (!string.IsNullOrEmpty(previousPageUrl))
            {
                category = await GetReferringCategoryAsync(previousPageUrl, productCategories);
            }
            //+++++++++++++++++++++++++++++++

            if (category == null)
            {
                return breadcrumbModel;
            }

            foreach (var catBr in await _categoryService.GetCategoryBreadCrumbAsync(category))
            {
                breadcrumbModel.CategoryBreadcrumb
                    .Add(
                        new CategorySimpleModel
                        {
                            Id = catBr.Id,
                            Name = await _localizationService.GetLocalizedAsync(catBr, x => x.Name),
                            SeName = await _urlRecordService.GetSeNameAsync(catBr),
                            IncludeInTopMenu = catBr.IncludeInTopMenu
                        });
            }

            return breadcrumbModel;
        }
 #endregion

     
    }
}