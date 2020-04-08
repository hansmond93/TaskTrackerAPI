using System;
using System.Collections.Generic;
using System.Text;
using AspNetCore.Totp;
using AspNetCore.Totp.Interface;

namespace Application.Services
{
    public interface ITotpService
    {
        int Generate(string key);
        bool Verify(int otp, string key);
    }

    public class TotpService : ITotpService
    {
        private readonly ITotpGenerator _totpGenerator;
        private const int ToleranceSeconds = 300;

        public TotpService(ITotpGenerator totpGenerator)
        {
            _totpGenerator = totpGenerator;
        }

        public int Generate(string key)
        {
            return _totpGenerator.Generate(key);
        }

        public bool Verify(int otp, string key)
        {
            return new TotpValidator(_totpGenerator).Validate(key, otp, ToleranceSeconds);
        }
    }
}