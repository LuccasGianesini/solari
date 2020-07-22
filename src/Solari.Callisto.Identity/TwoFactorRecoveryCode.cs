namespace Solari.Callisto.Identity
{
    // COPIED FROM https://github.com/matteofabbri/AspNetCore.Identity.Mongo
	public class TwoFactorRecoveryCode
	{
		public string Code { get; set; }
        public bool Redeemed { get; set; }
	}
}
