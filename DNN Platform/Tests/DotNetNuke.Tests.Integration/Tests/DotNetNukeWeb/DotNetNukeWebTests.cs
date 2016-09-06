﻿#region Copyright
//
// DotNetNuke® - http://www.dotnetnuke.com
// Copyright (c) 2002-2016
// by DotNetNuke Corporation
//
// Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated
// documentation files (the "Software"), to deal in the Software without restriction, including without limitation
// the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and
// to permit persons to whom the Software is furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in all copies or substantial portions
// of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED
// TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
// THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF
// CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER
// DEALINGS IN THE SOFTWARE.
#endregion

using System;
using System.Configuration;
using System.Net;
using System.Net.Http;
using System.Web;
using DotNetNuke.Tests.Integration.Framework;
using NUnit.Framework;

namespace DotNetNuke.Tests.Integration.Tests.DotNetNukeWeb
{
    [TestFixture]
    public class DotNetNukeWebTests : IntegrationTestBase
    {
        #region private data

        private readonly HttpClient _httpClient;

        private readonly TimeSpan _timeout = TimeSpan.FromSeconds(30);

        private const string GetMonikerQuery = "/API/Action/web/mobilehelper/monikers?moduleList=";
        private const string GetModuleDetailsQuery = "/API/Action/web/mobilehelper/moduledetails?moduleList=";

        public DotNetNukeWebTests()
        {
            var url = ConfigurationManager.AppSettings["siteUrl"];
            var siteUri = new Uri(url);
            _httpClient = new HttpClient { BaseAddress = siteUri, Timeout = _timeout };
        }

        #endregion

        #region tests

        [Test]
        [TestCase(GetMonikerQuery)]
        [TestCase(GetModuleDetailsQuery)]
        public void CallingHelperForAnonymousUserShouldReturnSuccess(string query)
        {
            var result = _httpClient.GetAsync(query + HttpUtility.UrlEncode("ViewProfile")).Result;
            var content = result.Content.ReadAsStringAsync().Result;
            ShowInfo(@"content => " + content);
            Assert.AreEqual(HttpStatusCode.OK, result.StatusCode);
        }

        #endregion

        #region helpers

        private static void ShowInfo(string info)
        {
            // Don't write anything to console when we run in TeamCity
            if (string.IsNullOrEmpty(Environment.GetEnvironmentVariable("TEAMCITY_VERSION")))
                Console.WriteLine(info);
        }

        #endregion
    }
}