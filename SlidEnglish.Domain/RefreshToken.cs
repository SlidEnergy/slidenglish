﻿using System;
using System.ComponentModel.DataAnnotations;

namespace SlidEnglish.Domain
{
    public class RefreshToken: IUniqueObject
    {
        public int Id { get; set; }

        [Required]
        public virtual User User { get; set; }

        [Required]
        public string DeviceId { get; set; }

        [Required]
        public string Token { get; set; }

        [Required]
        public DateTime ExpirationDate { get; set; }

        public bool IsExpired() => DateTime.Now > ExpirationDate;

		public RefreshToken() { }

		public RefreshToken(string deviceId, string token, User user)
		{
			DeviceId = deviceId;
			Token = token;
			ExpirationDate = DateTime.Today.AddYears(10);
			User = user;
		}

		public void Update(string deviceId, string token)
		{
			DeviceId = deviceId;
			Token = token;
			ExpirationDate = DateTime.Today.AddYears(10);
		}
    }
}
