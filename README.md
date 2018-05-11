# WeatherSystem

# README #

### What is this repository for? ###

An open source and free-to-use weather system package for Unity produced as an undergraduate dissertation piece. 

**Version:** 0.95
(*v1.0 release coming on 21/05/18*)

**Features:**
* Provides procedural and manually curated weather events. In procedural mode the weather is changable across the game world at any given instance.
* Provides a queryable interface for further customisation based on weather intensity, temperature and humidity for any given location in the game world. Also includes various delegate callbacks on key events; `OnWeatherChangeBeginEvent`, `OnWeatherChangeStep` and `OnWeatherChangeCompleteEvent`.
* Follows OOP principles to allow for custom extensions and modification and deals with weather in a generic way to allow for any weather type to be added.
* Includes example code and documentation.
* Is fully open source under the MIT licence (see below).

### What's here? ###

* Unity 2017.3 Project (Although use with other versions has been successful so worth testing irrelevant of Unity version)

### System Structure ###
The system is a tree based structure in which an IntensityData object is propagated from the `WeatherManager` to weather-property-specific (eg. precipitation) components.
![Intensity data flow throughout the weather system](https://i.imgur.com/ZUIVe17.png)

##### IntensityData #####
`IntensityData` is an object that is passed down the hierarchy and is modified at various levels by `AnimationCurves`. It includes the intensity of the current weather (0->1), the current Temperature (as an enum) and the current Humidity (as an enum). Further data can be queried from the WeatherManager or an inherited WeatherManager and IntensityData could be produced quite easily to include more data as required.

#### Customisation ####
The system provides all the logic for procedural (or manual) weather sequences. All you have to provide are the visual elements, whatever they may be, to be controlled by some derived `IntensityDrivenBehaviour`. Several example custom control components are provided for shaders and particle effects. The shader components are unlikely to work with an alternative shader (although the example component should go a good ways to providing how you could go about creating your own), however the particle effect controller is likely to be reusable in alternate circumstances.

Weather events, properties and the lookup table for temperature/humidity->weather type are all `ScriptableObjects`. Therefore, to create a new weather event, just add the new weather type from the "WeatherSystem->Weather Types" menu item and then create a new `WeatherEvent` by right-clicking in a project folder and clicking "Create->WeatherSystem->WeatherEvent". Other weather system items are also available under this menu - Weather sets, Weather lookup tables, and various weather properties.


### Bugs/Features/Pull Requests? ###

Just a quick question?
I can be found on [twitter](https://twitter.com/lordy) and occasionally [reddit](https://www.reddit.com/message/compose/?to=Developing_Developer).

Otherwise:
* Check if an [issue](https://github.com/Slord6/WeatherSystem/issues) already exists for your problem, if not [open a new issue](https://github.com/Slord6/WeatherSystem/issues/new). Please be as descriptive as possible.
* Pull requests and forks welcome.

### Licence ###

The code in this repository, unless otherwise stated, is distributed under the MIT Licence:
```
Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

     The above copyright notice and this permission notice shall be included in
     all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
THE SOFTWARE.
```
