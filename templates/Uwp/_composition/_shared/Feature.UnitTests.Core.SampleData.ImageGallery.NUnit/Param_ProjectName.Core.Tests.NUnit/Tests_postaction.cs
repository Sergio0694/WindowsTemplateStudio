﻿//{[{
using Param_RootNamespace.Core.Services;
//}]}

namespace Param_RootNamespace.Core.Tests.NUnit
{
    public class Tests
    {
        //^^
        //{[{

        // TODO WTS: Remove or update this once your app is using real data and not the SampleDataService.
        // This test serves only as a demonstration of testing functionality in the Core library.
        [Test]
        public void EnsureSampleDataServiceReturnsGalleryData()
        {
            var actual = SampleDataService.GetGallerySampleData();

            Assert.AreNotEqual(0, actual.Count);
        }
        //}]}
    }
}
