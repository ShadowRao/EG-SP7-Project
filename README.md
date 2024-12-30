# EG-SP7-Project
This is an Market & Community Hub, your one-stop platform for everything from decluttering to settling into a new city! Whether you're looking to sell your gadgets, lease your apartment, or lend a hand to a newcomer finding their way around, this is the place for you!
# Installation
This section provides instructions on how to run the application.

## Steps to Restore the Database

### 1. Clone the Repository

If you haven't already cloned the repository, do so by running:

```bash
git clone "https://github.com/ShadowRao/EG-SP7-Project.git"

```


### 2. Locate the Backup File

Check the repository for the database backup file.
- `CollectAndConnect.bak`

Note the location of the backup file for use in the restore command.

### 3. Open SQL Server Management Studio (SSMS)

1. Launch SSMS.
2. Connect to the SQL Server instance where you want to restore the database.

### 4. Restore the Database

1. In the **Object Explorer**, right-click on the **Databases** node and select **Restore Database...**.
2. In the **Restore Database** dialog:
   - Select **Device** under the **Source** section.
   - Click the **...** button next to **Device** to open the **Select backup devices** dialog.
   - Click **Add** and navigate to the location of the `.bak` file.
   - Select the `.bak` file and click **OK**.
3. Under the **Destination** section, specify the name of the database to restore.
4. Click **OK** to start the restore process.

### 5. Verify the Restore

After the restore completes, verify the database:

1. In **Object Explorer**, refresh the **Databases** node.
2. Expand the restored database and check that the tables, views, and other objects are present.

---

## Opening the Project in Visual Studio and Updating `appsettings.json`

### 1. Open the Project in Visual Studio

1. Launch **Visual Studio**.
2. Click on **File** > **Open** > **Project/Solution...**.
3. Navigate to the directory where the repository is cloned.
4. Select the solution file (usually ends with `.sln`) and click **Open**.
5. Install the dependencies
   Click on **Tools**  > **NuGet Package Manager** > **Manage NuGet Packages for Solution** and then install the following packages
   - MailKit
   - Microsoft.AspNetCore.Identity
   - Microsoft.AspNetCore.Mvc.Testing
   - Microsoft.EntityFrameworkCore
   - Microsoft.EntityFrameworkCore.Analyzers
   - Microsoft.EntityFrameworkCore.SqlServer
   - Microsoft.EntityFrameworkCore.Tools
   - MimeKit
   - Netonsoft.Json
   


### 2. Update the Server Name in `appsettings.json`

1. In **Solution Explorer**, locate the `appsettings.json` file, usually found in the root of the project.
2. Double-click the file to open it in the editor.
3. Find the section containing the database connection string. It typically looks like this:

   ```json
    "ConnectionStrings": {
    "DefaultConnection": "Server=AUTOR4gDAxq9VvQ; Database=CollectandConnect; Trusted_connection=true; TrustServerCertificate=true;"
    }
   ```

4. Update the `Server` value to the new server name:

   ```json
    "ConnectionStrings": {
    "DefaultConnection": "Server=YOUR_SERVER_NAME; Database=CollectandConnect; Trusted_connection=true; TrustServerCertificate=true;"
    }
   ```

5. Save the changes by pressing **Ctrl+S** or navigating to **File** > **Save**.
6. Rebuild the project to ensure the changes are applied:
   - Click **Build** > **Rebuild Solution** in the top menu.

### 3. Verify the Changes

1. Run the project by pressing **F5** or clicking the **Start** button.
2. Test the database connection to ensure the updated server name is being used.

---
