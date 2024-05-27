# SIDS.Plugin.Misc.BetterBreadCrumb
## Version 1.5 

## See BetterProductModelFactory and how it inherits from ProductModelFactory and overriding only the method you want to change rather than pullng in the entire class.
## Setting up the constructor takes a bit more time but way less code in the plugin class (only methods that need changing are in there).
## Should make future upgrades easier because you only need to worry about NOP offical changes to the overridden methods not to the whold class.
## Also, you can use the base class methods in your overridden methods if you need to.
## The ActionFilter is a bit of a hack but it works. I think you can use a newer way to get the previous page url and hence get rid of the aciton filter completely.

# TODO
## I think you can use a newer way to get the previous page url and hence get rid of the aciton filter completely
