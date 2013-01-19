using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using AcoustID.Util;

namespace AcoustID.Tests.Util
{
    [TestClass]
    public class Base64Test
    {
        [TestMethod]
        public void TestBase64Encode()
        {
            byte[] original = {
		        1, 0, 1, 207, 17, 181, 36, 18, 19, 37, 65, 15, 31, 197, 149, 161, 63, 33, 22,
		        60, 141, 27, 202, 35, 184, 47, 254, 227, 135, 135, 11, 58, 139, 208, 65, 127,
		        52, 167, 241, 31, 99, 182, 25, 159, 96, 70, 71, 160, 251, 168, 75, 132, 185,
		        112, 230, 193, 133, 252, 42, 126, 66, 91, 121, 60, 135, 79, 24, 185, 210, 28,
		        199, 133, 255, 240, 113, 101, 67, 199, 23, 225, 181, 160, 121, 140, 67, 123,
		        161, 229, 184, 137, 30, 205, 135, 119, 70, 94, 252, 71, 120, 150
	        };
            string encoded = "AQABzxG1JBITJUEPH8WVoT8hFjyNG8ojuC_-44eHCzqL0EF_NKfxH2O2GZ9gRkeg-6hLhLlw5sGF_Cp-Qlt5PIdPGLnSHMeF__BxZUPHF-G1oHmMQ3uh5biJHs2Hd0Ze_Ed4lg";
            Assert.AreEqual(encoded, Base64.Encode(Base64.ByteEncoding.GetString(original)));
        }

        [TestMethod]
        public void TestBase64Decode()
        {
            byte[] original = {
		        1, 0, 1, 207, 17, 181, 36, 18, 19, 37, 65, 15, 31, 197, 149, 161, 63, 33, 22,
		        60, 141, 27, 202, 35, 184, 47, 254, 227, 135, 135, 11, 58, 139, 208, 65, 127,
		        52, 167, 241, 31, 99, 182, 25, 159, 96, 70, 71, 160, 251, 168, 75, 132, 185,
		        112, 230, 193, 133, 252, 42, 126, 66, 91, 121, 60, 135, 79, 24, 185, 210, 28,
		        199, 133, 255, 240, 113, 101, 67, 199, 23, 225, 181, 160, 121, 140, 67, 123,
		        161, 229, 184, 137, 30, 205, 135, 119, 70, 94, 252, 71, 120, 150
	        };
            string encoded = "AQABzxG1JBITJUEPH8WVoT8hFjyNG8ojuC_-44eHCzqL0EF_NKfxH2O2GZ9gRkeg-6hLhLlw5sGF_Cp-Qlt5PIdPGLnSHMeF__BxZUPHF-G1oHmMQ3uh5biJHs2Hd0Ze_Ed4lg";
            Assert.AreEqual(Base64.ByteEncoding.GetString(original), Base64.Decode(encoded));
        }
    }
}
