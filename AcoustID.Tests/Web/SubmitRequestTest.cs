using AcoustID.Web;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AcoustID.Tests.Web
{
    public class SubmitRequestTest
    {
        [Test]
        public void TestWriteQueryString()
        {
            var request = new SubmitRequest("X", 200);

            request.MBID = "M";
            request.Title = "T";
            request.Artist = "A";
            request.Year = 2000;

            string expected = "&duration=200&fingerprint=X&mbid=M&track=T&artist=A&year=2000";

            using (var stream = new MemoryStream())
            using (var writer = new StreamWriter(stream))
            {
                request.WriteQueryString(writer);

                writer.Flush();

                stream.Position = 0;

                using (var reader = new StreamReader(stream))
                {
                    var actual = reader.ReadToEnd();

                    Assert.That(actual, Is.EqualTo(expected));
                }
            }

            request.Year = 0;

            expected = "duration.0=200&fingerprint.0=X&mbid.0=M&track.0=T&artist.0=A";

            using (var stream = new MemoryStream())
            using (var writer = new StreamWriter(stream))
            {
                request.WriteQueryString(writer, false, 0);

                writer.Flush();

                stream.Position = 0;

                using (var reader = new StreamReader(stream))
                {
                    var actual = reader.ReadToEnd();

                    Assert.That(actual, Is.EqualTo(expected));
                }
            }
        }
    }
}
