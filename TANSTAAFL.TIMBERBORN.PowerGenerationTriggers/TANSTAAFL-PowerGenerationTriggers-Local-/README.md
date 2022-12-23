# PowerGenerationTriggers

PowerGenerationTriggers is a Timberborn mod that reduces the District range after each drought.

# Usage

There are five values to alter, listed below:

* StartingRange: Initial district range (Default 75. Base game 70)
* ReductionAmount: By how much the distric will shrink each cycle (Default 1)
* CyclesPerReduction: After how many cycles the range will shrink (Default 1)
* MinimumRange: The lowest range possible (Default 35)
* MaximumRange: The highest range possible (Default 125)

Change the value of the variables in the config, which is probably in BepInEx\plugins\PowerGenerationTriggers\configs\PowerGenerationTriggers.json

If the config is not showing, try to launch the game and start up a save, that should create it.

# Issues

If there's a warning message in the log about a missing JSON property on game launch, delete the config file and restart the game to recreate it.

# Contributions
PRs are always welcome on the github page!

# Changelog

## v1.0.0 - 22.12.2022
- Initial release