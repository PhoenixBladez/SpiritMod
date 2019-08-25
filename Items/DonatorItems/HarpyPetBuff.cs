using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace SpiritMod.Items.DonatorItems
{
	class HarpyPetBuff : ModBuff
	{
		public static readonly int _type;

		public override void SetDefaults()
		{
			DisplayName.SetDefault("Harpy");
			Description.SetDefault("It will shoot at your enemies.");
			Main.buffNoTimeDisplay[Type] = true;
			Main.buffNoSave[Type] = true;
			Main.vanityPet[Type] = true;
		}

		public override void Update(Player player, ref int buffIndex)
		{
			player.buffTime[buffIndex] = 10;
			bool petNotSpawned = player.ownedProjectileCounts[Projectiles.DonatorItems.HarpyPet._type] <= 0;
			if (petNotSpawned && player.whoAmI == Main.myPlayer)
			{
				Projectile.NewProjectile(player.Center, Vector2.Zero, Projectiles.DonatorItems.HarpyPet._type, 0, 0f, player.whoAmI);
			}
		}
	}
}
