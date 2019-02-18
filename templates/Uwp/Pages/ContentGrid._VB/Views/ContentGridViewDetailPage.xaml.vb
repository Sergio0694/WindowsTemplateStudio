﻿Imports Param_RootNamespace.Services
Imports Microsoft.Toolkit.Uwp.UI.Animations

Namespace Views
    Public NotInheritable Partial Class ContentGridViewDetailPage
        Inherits Page

        Public Sub New()
            InitializeComponent()
        End Sub

        Protected Overrides Sub OnNavigatedTo(e As NavigationEventArgs)
            MyBase.OnNavigatedTo(e)
            Dim orderId As Long
            orderId = CType(e.Parameter, Long)
            ViewModel.Initialize(orderId)
        End Sub

        Protected Overrides Sub OnNavigatingFrom(e As NavigatingCancelEventArgs)
            MyBase.OnNavigatingFrom(e)
            If e.NavigationMode = NavigationMode.Back Then
                NavigationService.Frame.SetListDataItemForNextConnectedAnnimation(ViewModel.Item)
            End If
        End Sub
    End Class
End Namespace
