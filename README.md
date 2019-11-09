<p align="center">
  <img src="https://img.shields.io/github/license/pacojq/Ferret.svg?color=green" />
  <img src="https://img.shields.io/badge/version-0.0.1-blue.svg" />
</p>

# Ferret Engine

Ferret is a simple game engine developed in C# with [FNA](https://github.com/FNA-XNA/FNA), 
started as a project for the [devtober](https://twitter.com/devtober) challenge.


## Features

Ferret is still in *very* early stages of development. Some of the already existent 
functionality can be listed as follows:

- Composable Game Object structure
- Simple collision system
- Coroutine handling
- Particle system
- TTF font support
- Full control of the game for the programmer


## Getting Started

You can pull the Ferret repository with: `git clone --recursive https://github.com/pacojq/Ferret.git`.
Make sure you're using `--recursive`!

If you are using Ferret as a **git submodule**, execute: `git submodule update --init --recursive`.

Also, you may need the following:

- **DirectX SDK (June 2010)** for building shaders.
  - **On Windows** : Download from https://www.microsoft.com/en-us/download/details.aspx?id=6812
  - **On Linux/macOS:** Install using Wine and winetricks

- **FNA Dependencies**, already added as a submodule of Ferret


## License

Ferret is under [MIT License](/LICENSE).
Also, as FNA runs under the hood, please check out the [FNA License](https://github.com/FNA-XNA/FNA/tree/master/licenses).
