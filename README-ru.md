# Roger

My neural network, which I improve every day.

Copyright (c) 2025 Axolotl512, death_script. License: MIT.

An AI suitable for integration into a robot.

## Versions and Architecture

- **Quecto Roger 1 and below** — LP architecture, .NET 8.0.
- **Quecto Roger 2 and above** — MLP architecture, .NET 9.0.
- **Yocto Roger 2.1** — MLP architecture, .NET 9.0.

## Running Yocto Roger 2.1 on Windows

1. Install .NET 9.0.
2. Navigate to the following path: `Yocto_Roger_v._2._1/Yocto_Roger_v._2._1/bin/Release/net9.0`
3. Run `Yocto Roger v.2.1.exe`.
4. Edit the knowledge file (`knowledge.know`) or create your own (see the wiki for details).

### Training Parameters

- **LR (Learning Rate)** — the lower the value, the more precise the training.
- **Epochs** — the higher the value, the better the network assimilates information.
- **Knowledge File** — the file used to train the neural network.
- **DropOut** (only in `Yocto Roger v.2.1.exe`) — protection against overfitting. 
Recommended values: **5%** or **10%**. 
When using DropOut, a lower LR and a higher number of epochs are typically required.

## Running on Linux ARM (aarch64, Yocto Roger 2.1)

1. In the folder containing the files, execute:
`./Yocto_roger_2.1`
2. Edit the knowledge file `knowledge.know` (see the wiki for details). ### Training Parameters

- **LR (Learning Rate)** — the lower the value, the more precise the training.
- **Epochs** — the higher the value, the better the network assimilates information.
- **Knowledge File** — the file used to train the neural network.

> ⚠️ For the ARM version of Yocto Roger 2.1 / 3.0, installing .NET 9 is not required.

## Reminder
All announcements, news, and other project-related information will be posted on the Telegram channel: [Link to Channel](https://t.me/Axolotl1024)

Best regards, the Emotion Team ;)
