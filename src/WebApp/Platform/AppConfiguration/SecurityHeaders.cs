﻿using SWGW.WebApp.Platform.Settings;

namespace SWGW.WebApp.Platform.AppConfiguration;

internal static class SecurityHeaders
{
    internal static void AddSecurityHeaderPolicies(this HeaderPolicyCollection policies)
    {
        policies
            .AddFrameOptionsDeny()
            .AddXssProtectionBlock()
            .AddContentTypeOptionsNoSniff()
            .AddReferrerPolicyStrictOriginWhenCrossOrigin()
            .RemoveServerHeader();

        if (!string.IsNullOrEmpty(AppSettings.RaygunSettings.ApiKey))
            policies.AddReportingEndpoints(builder => builder.AddEndpoint("csp-endpoint",
                $"https://report-to-api.raygun.com/reports-csp?apikey={AppSettings.RaygunSettings.ApiKey}"));
    }
}
