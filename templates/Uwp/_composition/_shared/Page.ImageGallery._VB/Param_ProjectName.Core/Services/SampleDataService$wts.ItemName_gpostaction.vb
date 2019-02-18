﻿'{**
' This code block adds the method `GetSampleModelDataAsync()` to the SampleDataService of your project.
'**}
Namespace Services
    Public Module SampleDataService
'^^
'{[{

        Private _localResourcesPath As String

        Private _gallerySampleData As ObservableCollection(Of SampleImage)

        Public Sub Initialize(localResourcesPath As String)
            _localResourcesPath = localResourcesPath
        End Sub

        ' TODO WTS: Remove this once your image gallery page is displaying real data.
        Public Function GetGallerySampleData() As ObservableCollection(Of SampleImage)
            If _gallerySampleData Is Nothing Then
                _gallerySampleData = New ObservableCollection(Of SampleImage)()
                For i As Integer = 1 To 10
                    _gallerySampleData.Add(New SampleImage() With {
                        .ID = $"{i}",
                        .Source = $"{_localResourcesPath}/SampleData/SamplePhoto{i}.png",
                        .Name = $"Image sample {i}"
                    })
                Next
            End If
            Return _gallerySampleData
        End Function
'}]}
    End Module
End Namespace
