# PLAN: INews Integration Service Extraction

## Goal
Create a dedicated `INewsService` class to handle iNews server interactions.
**Scope**: 
- **IN**: iNews Server Connection, Queue Navigation, Story Retrieval (as DataTable for UI compatibility), XML Export.
- **OUT**: UI logic, CG/TroiTin processing, modifications to `ServerAPI`/`iNewsData`.

## 🏗️ Design Proposal

### 1. Location & Structure
- **File**: `API_iNews/Services/INewsService.cs`
- **Namespace**: `API_iNews` (Requires `using TTDH;` to access App_Code classes).
- **Interface**: `IINewsService` (Included in the same file for simplicity).

### 2. Class Definition

```csharp
using TTDH; // For iNewsData, ProcessingXMl2Class

namespace API_iNews
{
    public interface IINewsService
    {
        // Connection
        Task<bool> ConnectAsync();
        void Disconnect();
        bool IsConnected { get; }

        // Navigation
        // Returns list of queue names (e.g. "BAN_TIN_12H")
        Task<List<string>> GetQueueChildrenAsync(string parentQueueName);

        // Data Retrieval (UI Binding)
        // Returns DataTable because InewsForm.dataGridView1 binds directly to it.
        // Internally calls iNewsData.GetStoriesBoard -> ProcessingXMl2Class.GetDataRows
        Task<System.Data.DataTable> GetStoriesForUiAsync(string queueName);

        // Export
        // Handles the loop to write story_{i}.xml files
        void ExportStoriesToXml(List<string> rawStories, string destinationPath);
        
        // Raw Access (For custom processing if needed)
        Task<List<string>> GetRawStoriesAsync(string queueName);
    }

    public class INewsService : IINewsService
    {
        // Uses ConfigurationManager.AppSettings internally
        // Wraps iNewsData methods
    }
}
```

### 3. Migration Plan (Step-by-Step)

#### Step 1: Create the Service
- **Action**: Create `API_iNews/Services/INewsService.cs`.
- **Content**: Implement the interface and class using existing `iNewsData` and `ProcessingXMl2Class` logic.
- **Verification**: Compile. The project should build without errors.

#### Step 2: Integrate Connection (Low Risk)
- **Action**: In `InewsForm.cs`, instantiate `_inewsService`.
- **Action**: Replace code in `ConnectServerToLoadDataAsync` to use `_inewsService.ConnectAsync()`.
- **Verification**: Run app. Verify it connects to server on startup.

#### Step 3: Integrate Navigation (Low Risk)
- **Action**: Update `LoadTreeStoriesAsync` to call `_inewsService.GetQueueChildrenAsync()`.
- **Verification**: Run app. Verify TreeView populates correct folders.

#### Step 4: Integrate Data Retrieval (Medium Risk)
- **Action**: Update `treeView1_AfterSelect` to call `_inewsService.GetStoriesForUiAsync()`.
- **Verification**: Run app. Click a queue. Verify DataGridView loads stories.

#### Step 5: Integrate Export (Low Risk)
- **Action**: Update `RefreshDataiNewsAsync` (Export logic) to use `_inewsService.ExportStoriesToXml`.
- **Verification**: Run app. Trigger export/refresh. Check output folder for `story_*.xml` files.

## 📂 Deliverables
- [NEW] `API_iNews/Services/INewsService.cs`
