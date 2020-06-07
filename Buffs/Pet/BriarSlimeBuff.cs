using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace SpiritMod.Buffs.Pet
{
    public class BriarSlimeBuff : ModBuff
	{
		public override void SetDefaults()
		{
			DisplayName.SetDefault("Glowing Briarthorn Slime");
            Description.SetDefault("'Don't get too close'");
            Main.buffNoTimeDisplay[Type] = true;
            Main.lightPet[Type] = true;
        }

		public override void Update(Player player, ref int buffIndex)
		{
			player.buffTime[buffIndex] = 18000;
			player.GetSpiritPlayer().briarSlimePet = true;

			bool petProjectileNotSpawned = player.ownedProjectileCounts[ModContent.ProjectileType<BriarSlimePet>()] <= 0;
			if (petProjectileNotSpawned && player.whoAmI == Main.myPlayer)
			{
				Projectile.NewProjectile(player.Center, Vector2.Zero, ModContent.ProjectileType<BriarSlimePet>(), 0, 0f, player.whoAmI);
			}
		}
	}
}