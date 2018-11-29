using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace Vbtonsoft.AuthenticationCore.Core
{
    public class YepAuthenticationHandler : AuthenticationHandler<YepAuthenticationSchemeOptions>
    {
        public YepAuthenticationHandler(IOptionsMonitor<YepAuthenticationSchemeOptions> options, ILoggerFactory logger, UrlEncoder encoder, ISystemClock clock) : base(options, logger, encoder, clock)
        {
        }
        protected new YepAuthenticationEvents Events { get => (YepAuthenticationEvents)base.Events; set => base.Events = value; }

        protected override Task<object> CreateEventsAsync()
        {
            return Task.FromResult<object>(new YepAuthenticationEvents());
        }

        protected virtual ClaimsIdentity CreateClaimsIdentity(string identityName)
        {
            if (string.IsNullOrEmpty(identityName))
            {
                throw new ArgumentException("identityName cannot be bull or empty.", nameof(identityName));
            }
            ClaimsIdentity yepClaims = new ClaimsIdentity(Scheme.Name, ClaimTypes.Name, ClaimTypes.Role);
            yepClaims.AddClaim(new Claim(ClaimTypes.Name, identityName, ClaimValueTypes.String, Options.ClaimsIssuer));
            foreach (Claim claim in Options.Claims)
            {
                Claim claim2 = new Claim(claim.Type ?? ClaimTypes.Role, claim.Value, claim.ValueType ?? ClaimValueTypes.String, claim.Issuer ?? Options.ClaimsIssuer, claim.OriginalIssuer, yepClaims);
                if (claim.Properties.Count > 0)
                {
                    foreach (KeyValuePair<string, string> kv in claim.Properties)
                    {
                        claim2.Properties[kv.Key] = kv.Value;
                    }
                }
                yepClaims.AddClaim(claim2);
            }
            return yepClaims;
        }

        private async Task<AuthenticateResult> OnHandleAuthenticateAsync(MessageReceivedContext context)
        {
            TokenValidatedContext validatedContext = new TokenValidatedContext(Context, Scheme, Options)
            {
                Token = context.Token
            };
            await Events.TokenValidated(validatedContext);
            context.Success();
            return context.Result;

        }

        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            MessageReceivedContext context = new MessageReceivedContext(Context, Scheme, Options);
            await Events.MessageReceived(context);
            if (context.Result != null)
            {
                return context.Result;
            }
            TokenValidateContext validateContext = new TokenValidateContext(Context, Scheme, Options)
            {
                Token = context.Token
            };
            SecurityTokenValidate validate = await Events.TokenValidate(validateContext);
            if (validateContext.Result != null)
            {
                return validateContext.Result;
            }
            if (validate.TokenSuccess)
            {
                context.InternalUser = CreateClaimsIdentity(validate.UserProvider.IdentityName);
                return await OnHandleAuthenticateAsync(context);
            }
            return AuthenticateResult.NoResult();
        }

        protected override async Task HandleChallengeAsync(AuthenticationProperties properties)
        {
            AuthenticateResult authenticateResult = await this.HandleAuthenticateOnceSafeAsync();
            YepChallengeContext eventContext = new YepChallengeContext(Context, Scheme, Options, properties);
            await Events.Challenge(eventContext);
            if (!eventContext.Handled)
            {
                Response.StatusCode = 401;
                if (string.IsNullOrEmpty(eventContext.Error) && string.IsNullOrEmpty(eventContext.ErrorDescription) && string.IsNullOrEmpty(eventContext.ErrorUri))
                {
                    HeaderDictionaryExtensions.Append(Response.Headers, "WWW-Authenticate", Options.Challenge);
                }
                else
                {
                    StringBuilder sb = new StringBuilder(Options.Challenge);
                    if (Options.Challenge.IndexOf(" ", StringComparison.Ordinal) > 0)
                    {
                        sb.Append(',');
                    }
                    if (!string.IsNullOrEmpty(eventContext.Error))
                    {
                        sb.Append(" error=\"");
                        sb.Append(eventContext.Error);
                        sb.Append("\"");
                    }
                    if (!string.IsNullOrEmpty(eventContext.ErrorDescription))
                    {
                        if (!string.IsNullOrEmpty(eventContext.Error))
                        {
                            sb.Append(",");
                        }
                        sb.Append(" error_description=\"");
                        sb.Append(eventContext.ErrorDescription);
                        sb.Append('"');
                    }
                    if (!string.IsNullOrEmpty(eventContext.ErrorUri))
                    {
                        if (!string.IsNullOrEmpty(eventContext.Error) || !string.IsNullOrEmpty(eventContext.ErrorDescription))
                        {
                            sb.Append(",");
                        }
                        sb.Append(" error_uri=\"");
                        sb.Append(eventContext.ErrorUri);
                        sb.Append('"');
                    }
                    HeaderDictionaryExtensions.Append(Response.Headers, "WWW-Authenticate", sb.ToString());
                }
            }
        }
    }
}
