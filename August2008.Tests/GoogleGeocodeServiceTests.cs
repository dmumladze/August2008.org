using System;
using System.IO;
using August2008;
using August2008.Common.Interfaces;
using August2008.Data;
using August2008.Model;
using August2008.Services;
using August2008.Tests.Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace August2008.Tests
{
    [TestClass]
    public class GoogleGeocodeServiceTests
    {        
        [TestMethod]
        public void Test()
        {
            var service = UnityHelper.Resolve<IGeocodeService>();

            var xml = File.ReadAllText(@"TestData\PayPalVariables.payment.xml");
            var vars = xml.FromXml<PayPalVariables>();
            GeoLocation geo;
            var success = service.TryGetGeoLocation(vars, out geo);
        }
    }
}
