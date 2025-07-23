# Drilling Machine Digital Twin

## Introduction
This project presents a simplified digital twin of a drilling machine, developed with the Unity game engine as part of an engineering internship at Saipem SA. 

The digital twin replicates the essential functions of a real drilling machine in a virtual environment, enabling users to interactively explore its components and operations in an intuitive way. 

In addition to interaction, the system supports real-time visualization of sensor data collected from the physical drilling machine, allowing users to monitor its performance live while engaging with the simulation. 

This integration of monitoring and virtual modeling offers a safe and controlled platform for training, facilitates a deeper understanding of machine behavior, and enhances operational insights.

## Installation
* Download the installer ([DM-DigitalTwinSetup.exe](https://github.com/Starlight-25/DrillingMachine-DigitalTwin/releases/download/v1.0/DM-DigitalTwinSetup.exe))
* Run the installer (DM-DigitalTwinSetup.exe) and follow the instructions
* Follow the instructions of the installer:
  * Choose your preferred language
  * Accept the terms and conditions
  * Select the installation directory (default: C:\Program Files\DrillingMachine-DigitalTwin)
  * Choose whether to create a desktop shortcut or not by checking/unchecking the option
  * Click Install
* After installation:
  * Optionally check ”Launch Drilling Machine Digital Twin”
  * Click Finish to close the installer

You can launch the software in two ways:
* From the desktop (if shortcut was created):  
  Double-click the Drilling Machine Digital Twin icon.
* From the Start Menu:  
  Go to Start > Drilling Machine Digital Twin > Open

## Uninstallation
* Open Control Panel > Programs > Uninstall a program
* Select DrillingMachine-DigitalTwin and click Uninstall  
Or:
* Use the Uninstall DrillingMachine-DigitalTwin shortcut from the Start Menu

## Commands
### In Drilling Mode
| Action                                          | Key                               |
|-------------------------------------------------|-----------------------------------|
| Return / Settings Menu                          | ESC                               |
| Open Parameter Menu                             | Tab                               |
| Move camera view                                | Mouse Right Click + Mouse Movement |
| Height Navigation                               | W/S                               |
| Zoom in/out                                     | Mouse scroll                      |
| Select Slip Table                               | 1                                 |
| Select Rotary Table                             | 2                                 |
| Reset selection                                 | 3                                 |
| Lock the selected table to the Kelly bar        | L                                 |
| Move selected upward                            | Arrow key Up                      |
| Move selected downward                          | Arrow Key Down                    |
| Change Drilling Leader Tower details visibility | V                                 |
| Change Terrain Layer visibility                 | T                                 |

### In Replay Mode
| Action                                          | Key                                |
|-------------------------------------------------|------------------------------------|
| Return / Settings Menu                          | ESC                                |
| Move camera view                                | Mouse Right Click + Mouse Movement |
| Height Navigation                               | W/S                                |
| Zoom in/out                                     | Mouse scroll                       |
| Change Drilling Leader Tower details visibility | V                                  |
| Change Terrain Layer visibility                 | T                                  |
| Pause/Resume                                    | Space                              |


## Mechanics
### Main Menu
Upon launching the software, users can choose between two modes: Interactive Mode and Replay Mode, by selecting the corresponding button.

Additionally, users are directed to the Settings Menu, where various configurable options are available. These include:
* Display Settings: Screen mode and refresh rate.
* Navigation Sensitivity: Mouse control, scroll sensitivity, and height navigation sensitivity.
* Graphics Settings: Fog distance and sensor visibility.

Users also have access to the Credits Menu, where information regarding the various assets and development tools utilized in the creation of the software is available.

### Interactive Mode
In Interactive Mode, users directly interact with and control the drilling machine through a set of commands.

To move the drilling machine, the user must select one of the two available tables: the Slip Table or the Rotary Table. When one of the tables is selected it is highlighted in white.

When a table is locked the user can see (when selected) the table highlighted in orange and the Kelly bar and drill bit move together with the selected table, enabling the drilling operation. If the table is unlocked (highlighted in white) these tables move along the Kelly bar when the user triggers the movements with the associated keys. Note that when both tables are locked none of the tables will be able to move.

Height navigation is divided into three distinct layers: Surface, Underwater, and Underground. Within the underground layer, users can observe the different subsea soil.

A Parameters Menu is accessible by pressing the TAB key, allowing adjustment of several drilling machine and subsea soil parameters. These include:
* Time speed, enabling acceleration or deceleration of simulated time.
* Drilling velocity.
* Rotation velocity.
* Terrain layer parameters such as the required weight for each layer and their respective depths.

The Settings Menu can be accessed at any time by pressing the ESC key, which also allows returning to the main menu.

Sensors installed on the drilling machine are interactive and can be selected with the mouse when highlighted in blue. Selecting a sensor displays its data evolution through a line chart.

### Replay Mode
The Replay Mode enables users to review and monitor sensor data and observe the drilling process and installation over time.

To use this mode, a properly formatted CSV file containing the required data must be provided. The file must include a header row with the following columns:
Date, DLT_B, DLT_C, DM, ST_Height, RT_Height, DrillBit_Height, DrillBit_Rotation, ST_Load, ST_Temp, RT_Load, RT_Temp, WeightOnBit, DrillingVelocity, DB_Torque, Layer.

Sensor data visualization is available through line charts similar to those in Interactive Mode. Users can navigate through the timeline using a slider to move forward or backward to specific timestamps.

Replay playback speed can be adjusted, functioning like a video player within a 3D environment.

Subsea soil corresponding to the data provided in the CSV file are also displayed.
The Settings Menu in Replay Mode offers the same configuration options as in Interactive Mode.

## License
End-User License Agreement (EULA)

This software is licensed, not sold. By installing or using the software, you agree to the following terms:

1. You may use this software for personal entertainment.
2. You may not redistribute, modify, or decompile this software.
3. All content, including code, art, and audio, is owned by Lin Jérôme.
4. This software is provided "as is", without warranty of any kind.

© 2025 Saipem SA. All rights reserved.
