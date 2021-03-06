﻿Imports System.Windows.Forms

Public Class frmAdmin
    Private Sub FrmAdmin_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ' set values for admin panel
        cmbSpawnItem.Items.Clear()

        ' Add the names
        For i = 1 To MAX_ITEMS
            cmbSpawnItem.Items.Add(i & ": " & Trim$(Item(i).Name))
        Next
    End Sub

#Region "Moderation"
    Private Sub BtnAdminWarpTo_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnAdminWarpTo.Click
        Dim n As Integer

        If GetPlayerAccess(MyIndex) < AdminType.Mapper Then
            AddText("You need to be a high enough staff member to do this!", QColorType.AlertColor)
            Exit Sub
        End If

        If Len(Trim$(txtAdminMap.Text)) < 1 Then
            Exit Sub
        End If

        If Not IsNumeric(Trim$(txtAdminMap.Text)) Then
            Exit Sub
        End If

        n = Trim$(txtAdminMap.Text)

        ' Check to make sure its a valid map #
        If n > 0 And n <= MAX_MAPS Then
            WarpTo(n)
        Else
            AddText("Invalid map number.", ColorType.BrightRed)
        End If
    End Sub

    Private Sub BtnAdminBan_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnAdminBan.Click
        If GetPlayerAccess(MyIndex) < AdminType.Mapper Then
            AddText("You need to be a high enough staff member to do this!", QColorType.AlertColor)
            Exit Sub
        End If

        If Len(Trim$(txtAdminName.Text)) < 1 Then
            Exit Sub
        End If

        SendBan(Trim$(txtAdminName.Text))
    End Sub

    Private Sub BtnAdminKick_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnAdminKick.Click
        If GetPlayerAccess(MyIndex) < AdminType.Mapper Then
            AddText("You need to be a high enough staff member to do this!", QColorType.AlertColor)
            Exit Sub
        End If

        If Len(Trim$(txtAdminName.Text)) < 1 Then
            Exit Sub
        End If

        SendKick(Trim$(txtAdminName.Text))
    End Sub

    Private Sub BtnAdminWarp2Me_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnAdminWarp2Me.Click
        If GetPlayerAccess(MyIndex) < AdminType.Mapper Then
            AddText("You need to be a high enough staff member to do this!", QColorType.AlertColor)
            Exit Sub
        End If

        If Len(Trim$(txtAdminName.Text)) < 1 Then
            Exit Sub
        End If

        If IsNumeric(Trim$(txtAdminName.Text)) Then
            Exit Sub
        End If

        WarpToMe(Trim$(txtAdminName.Text))
    End Sub

    Private Sub BtnAdminWarpMe2_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnAdminWarpMe2.Click
        If GetPlayerAccess(MyIndex) < AdminType.Mapper Then
            AddText("You need to be a high enough staff member to do this!", QColorType.AlertColor)
            Exit Sub
        End If

        If Len(Trim$(txtAdminName.Text)) < 1 Then
            Exit Sub
        End If

        If IsNumeric(Trim$(txtAdminName.Text)) Then
            Exit Sub
        End If

        WarpMeTo(Trim$(txtAdminName.Text))
    End Sub

    Private Sub BtnAdminSetAccess_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnAdminSetAccess.Click
        If GetPlayerAccess(MyIndex) < AdminType.Creator Then
            AddText("You need to be a high enough staff member to do this!", QColorType.AlertColor)
            Exit Sub
        End If

        If Len(Trim$(txtAdminName.Text)) < 2 Then
            Exit Sub
        End If

        If IsNumeric(Trim$(txtAdminName.Text)) Or cmbAccess.SelectedIndex < 0 Then
            Exit Sub
        End If

        SendSetAccess(Trim$(txtAdminName.Text), cmbAccess.SelectedIndex)
    End Sub

    Private Sub BtnAdminSetSprite_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnAdminSetSprite.Click
        If GetPlayerAccess(MyIndex) < AdminType.Mapper Then
            AddText("You need to be a high enough staff member to do this!", QColorType.AlertColor)
            Exit Sub
        End If

        If Len(Trim$(txtAdminSprite.Text)) < 1 Then
            Exit Sub
        End If

        If Not IsNumeric(Trim$(txtAdminSprite.Text)) Then
            Exit Sub
        End If

        SendSetSprite(Trim$(txtAdminSprite.Text))
    End Sub
#End Region

#Region "Editors"
    Private Sub BtnMapEditor_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnMapEditor.Click

        If GetPlayerAccess(MyIndex) < AdminType.Mapper Then
            AddText("You need to be a high enough staff member to do this!", QColorType.AlertColor)
            Exit Sub
        End If

        SendRequestEditMap()
    End Sub
#End Region

#Region "Map Report"
    Private Sub BtnMapReport_Click(sender As Object, e As EventArgs) Handles btnMapReport.Click
        If GetPlayerAccess(MyIndex) < AdminType.Mapper Then
            AddText("You need to be a high enough staff member to do this!", QColorType.AlertColor)
            Exit Sub
        End If
        SendRequestMapreport()
    End Sub

    Private Sub LstMaps_DoubleClick(sender As Object, e As EventArgs) Handles lstMaps.DoubleClick
        If GetPlayerAccess(MyIndex) < AdminType.Mapper Then
            AddText("You need to be a high enough staff member to do this!", QColorType.AlertColor)
            Exit Sub
        End If

        ' Check to make sure its a valid map #
        If lstMaps.FocusedItem.Index + 1 > 0 And lstMaps.FocusedItem.Index + 1 <= MAX_MAPS Then
            WarpTo(lstMaps.FocusedItem.Index + 1)
        Else
            AddText("Invalid map number: " & lstMaps.FocusedItem.Index + 1, QColorType.AlertColor)
        End If
    End Sub
#End Region

#Region "Misc"
    Private Sub CmbSpawnItem_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cmbSpawnItem.SelectedIndexChanged
        If Item(cmbSpawnItem.SelectedIndex + 1).Type = ItemType.Currency Or Item(cmbSpawnItem.SelectedIndex + 1).Stackable = 1 Then
            scrlSpawnItemAmount.Enabled = True
            scrlSpawnItemAmount.Maximum = 1000
            Exit Sub
        End If
        scrlSpawnItemAmount.Enabled = False
    End Sub

    Private Sub ScrlSpawnItemAmount_Scroll(ByVal sender As Object, ByVal e As ScrollEventArgs) Handles scrlSpawnItemAmount.Scroll
        lblSpawnItemAmount.Text = "Amount: " & scrlSpawnItemAmount.Value
    End Sub

    Private Sub BtnSpawnItem_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnSpawnItem.Click
        If GetPlayerAccess(MyIndex) < AdminType.Creator Then
            AddText("You need to be a high enough staff member to do this!", QColorType.AlertColor)
            Exit Sub
        End If

        SendSpawnItem(cmbSpawnItem.SelectedIndex + 1, scrlSpawnItemAmount.Value)
    End Sub

    Private Sub BtnLevelUp_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnLevelUp.Click
        If GetPlayerAccess(MyIndex) < AdminType.Developer Then
            AddText("You need to be a high enough staff member to do this!", QColorType.AlertColor)
            Exit Sub
        End If

        SendRequestLevelUp()

    End Sub

    Private Sub BtnALoc_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnALoc.Click
        If GetPlayerAccess(MyIndex) < AdminType.Mapper Then
            AddText("You need to be a high enough staff member to do this!", QColorType.AlertColor)
            Exit Sub
        End If

        BLoc = Not BLoc
    End Sub

    Private Sub BtnRespawn_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnRespawn.Click
        If GetPlayerAccess(MyIndex) < AdminType.Mapper Then
            AddText("You need to be a high enough staff member to do this!", QColorType.AlertColor)
            Exit Sub
        End If

        SendMapRespawn()
    End Sub

#End Region

End Class