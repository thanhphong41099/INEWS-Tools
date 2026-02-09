# iNews Tool Logic Workflow

## Overview
This document outlines the logical flow of the iNews Tool application, specifically focusing on the process from connecting to the server to exporting data.

## 1. Startup & Connection
- **Trigger**: Application Launch (`FormInews_Load`) or "Connect Server" button click.
- **Action**: `ConnectToServer()`
- **Process**:
    1.  Calls `_service.ConnectAsync()` to establish a connection with the iNews Server.
    2.  If successful:
        -   Updates UI status to "Connected".
        -   Calls `LoadTree(ROOT_QUEUE)` to fetch and display the root queue structure in the `TreeView`.

## 2. Queue Selection (User Interaction)
- **Trigger**: User clicks a node in the `TreeView`.
- **Action**: `treeView1_AfterSelect` -> `SelectQueue(TreeNode)`
- **Process**:
    1.  Identify the queue path from `node.Tag` (failsafe to `node.Text`).
    2.  Set the global `_selectedQueue` variable.
    3.  **Auto-Load Data**: Immediately calls `LoadStoriesToGrid()` to fetch content.

## 3. Data Retrieval & Parsing (Core Logic)
- **Trigger**: Called by `SelectQueue` or "Export" button.
- **Action**: `LoadStoriesToGrid` / `CreateVideoIdTable`
- **Process**:
    1.  **Fetch Raw Data**: Calls `_service.GetRawStoriesAsync(_selectedQueue)` to get a list of raw XML strings for all stories in the queue.
    2.  **Parse Data (`CreateVideoIdTable`)**:
        -   **Read Config**: Reads `ConfigurationManager.AppSettings["Fields"]` (e.g., `"page-number,title,video-id"`).
        -   **Dynamic Columns**: Creates columns in a `DataTable` matching these config fields.
        -   **Extract Content**: Iterates through each raw XML story:
            -   Uses `ExtractField(xml, field)` to find content.
            -   **Regex Logic**: Searches for tags with `id="fieldName"` (e.g., `<string id="title">Content</string>`).
            -   *Note: Handles XML attributes and excludes self-closing tags.*
    3.  **Display**: Binds the resulting `DataTable` to `dataGridView1`.

## 4. Data Export (XML Generation)
- **Trigger**: User clicks "Export Video List" button.
- **Action**: `btnVideoID_Click` -> `ExtractVideoIds`
- **Process**:
    1.  **Re-Fetch Data**: Ensures freshest data by calling `GetRawStoriesAsync` again.
    2.  **Re-Parse**: Calls `CreateVideoIdTable` to generate the `DataTable` based on current config.
    3.  **Determine Path**:
        -   Base Folder: `ConfigurationManager.AppSettings["FolderToSave"]`.
        -   Sub-folder: Named after the selected Queue (sanitized).
        -   File Name: Fixed as `videoID_list.xml`.
    4.  **Write File (`SaveDataTableToXml`)**:
        -   Creates an XML structure:
            ```xml
            <Stories>
                <Story>
                    <[ConfigFieldName]>[Value]</[ConfigFieldName]>
                    ...
                </Story>
            </Stories>
            ```
        -   Saves to disk.
    5.  **Notify**: specific success message with file path.
