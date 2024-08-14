# Navigator CLI

Navigator is a console-based file explorer tool for Arch Linux written in C#. It is still very much a work in progress and is far from finished. The tool allows users to navigate directories, view and select files, and execute commands on files using a configurable CLI interface.

## Current Status

This project is **VERY MUCH WORK IN PROGRESS** and **VERY UNFINISHED**. It is currently in the alpha stage with basic functionality. Features and stability may change frequently.

## Features

- Display current working directory.
- List directories and files with syntax highlighting.
- Navigate directories using arrow keys.
- Open files using an external command.
- Exit tool and change the terminal's PWD.

## Installation

1. Clone the repository:
    ```bash
    git clone https://github.com/yourusername/navigator-cli.git
    ```

2. Navigate to the project directory:
    ```bash
    cd navigator-cli
    ```

3. Restore dependencies and build the project:
    ```bash
    dotnet restore
    dotnet build
    ```

4. Run the application:
    ```bash
    dotnet run
    ```

## Usage

Upon running, Navigator will:

1. Display the current working directory at the top.
2. List all folders and files in the current directory, sorted alphabetically.
3. Allow navigation using the arrow keys:
    - **Up Arrow**: Move up the list.
    - **Down Arrow**: Move down the list.
    - **Right Arrow / Enter**: Enter the selected directory or open the selected file.
    - **Left Arrow**: Go back to the parent directory.
    - **Exit Key**: Exit the tool and change the terminal's PWD to the last directory used.

If you select a file and press "Open", you will be prompted to enter a command to open the file. The tool will then exit and execute the command with the file path.

## Configuration

The tool uses a configuration file located at `~/.config/navigator/config.toml`. The config file can be edited to customize:

- Colors for directories, files, and selected items.
- Keybindings for navigation and actions.

## License

Navigator is licensed under the Mozilla Public License 2.0. See LICENSE for more details.
## Disclaimer

This tool is still under development and should be used with caution. Some features might not work as expected, and data loss or system instability could occur. Please use it at your own risk.
## Contact

For any questions or issues, please open an issue on the GitHub repository.

