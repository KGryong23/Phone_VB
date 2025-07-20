Public Module PermissionDescriptions
    Public ReadOnly PermissionDesc As New Dictionary(Of PermissionEnum, String) From {
        {PermissionEnum.view_phones, "View phone list"},
        {PermissionEnum.detail_phone, "View phone details"},
        {PermissionEnum.add_phone, "Add new phone"},
        {PermissionEnum.update_phone, "Update phone"},
        {PermissionEnum.delete_phone, "Delete phone"},
        {PermissionEnum.view_stocktrans, "View stock transactions"},
        {PermissionEnum.view_roles, "View roles"},
        {PermissionEnum.view_users, "View users"},
        {PermissionEnum.import_stock, "Import stock"},
        {PermissionEnum.export_stock, "Export stock"}
    }
End Module
