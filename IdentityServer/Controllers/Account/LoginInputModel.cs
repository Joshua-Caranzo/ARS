// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using System;
using System.ComponentModel.DataAnnotations;

namespace Identity.API.Controllers.Account
{
    public class LoginInputModel
    {
        [Required]
        public string UserName { get; set; }
        [Required]
        public string Password { get; set; }
        public string GeneratedCode { get; set; }
        public bool RememberLogin { get; set; }
        public string ReturnUrl { get; set; }

        public string Hash { get; set; }
        public string imageUrl { get; set; }
        public bool IsGoogleAuth { get; set; }
        public bool? MFA { get; set; }
        public DateTime? MFADate { get; set; }
        public string errorMessage { get; set; }
    }
}