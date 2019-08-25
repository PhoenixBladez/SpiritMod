using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace SpiritMod.Items.DonatorItems
{
	class LoomingPresence : ModBuff
	{
		public static readonly int _type;

		public override void SetDefaults()
		{
			DisplayName.SetDefault("Looming Presence");
			Description.SetDefault("It seems to attract a lot of attention.");
			Main.buffNoTimeDisplay[Type] = true;
			Main.buffNoSave[Type] = true;
			Main.vanityPet[Type] = true;
		}

		public override void Update(Player player, ref int buffIndex)
		{
			player.buffTime[buffIndex] = 10;
			bool petNotSpawned = player.ownedProjectileCounts[Projectiles.DonatorItems.DemonicBlob._type] <= 0;
			if (petNotSpawned && player.whoAmI == Main.myPlayer)
			{
				Projectile.NewProjectile(player.Center, Vector2.Zero, Projectiles.DonatorItems.DemonicBlob._type, 0, 0f, player.whoAmI);
			}
		}
	}
}
