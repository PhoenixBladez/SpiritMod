using Microsoft.Xna.Framework;
using SpiritMod.Dusts;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Projectiles
{
	class MiscEffectProjectile : GlobalProjectile
	{
		public override void ModifyHitNPC(Projectile projectile, NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
		{
			Player player = Main.player[projectile.owner];
			MyPlayer modPlayer = player.GetModPlayer<MyPlayer>();

			// Ace of Spades
			if (modPlayer.AceOfSpades && crit)
			{
				damage = (int)(damage * 1.1f + 0.5f);
				for (int i = 0; i < 3; i++)
					Dust.NewDust(target.position, target.width, target.height, ModContent.DustType<SpadeDust>(), 0, -0.8f);
			}

			if (modPlayer.AceOfClubs && crit && !target.friendly && target.lifeMax > 15 && !target.SpawnedFromStatue && target.type != NPCID.TargetDummy)
			{
				int money = (int)(300 * MathHelper.Clamp((float)damage / target.lifeMax, 1 / 295f, 1f));
				for (int i = 0; i < 3; i++)
					Dust.NewDust(target.position, target.width, target.height, ModContent.DustType<ClubDust>(), 0, -0.8f);

				if (money / 1000000 > 0)
					ItemUtils.NewItemWithSync(projectile.owner, (int)target.position.X, (int)target.position.Y, target.width, target.height, ItemID.PlatinumCoin, money / 1000000);

				money %= 1000000;
				if (money / 10000 > 0)
					ItemUtils.NewItemWithSync(projectile.owner, (int)target.position.X, (int)target.position.Y, target.width, target.height, ItemID.GoldCoin, money / 10000);

				money %= 10000;
				if (money / 100 > 0)
					ItemUtils.NewItemWithSync(projectile.owner, (int)target.position.X, (int)target.position.Y, target.width, target.height, ItemID.SilverCoin, money / 100);

				money %= 100;
				if (money > 0)
					ItemUtils.NewItemWithSync(projectile.owner, (int)target.position.X, (int)target.position.Y, target.width, target.height, ItemID.CopperCoin, money);
			}

			// Briar Set Bonus
			if (modPlayer.reachSet && target.life <= target.life / 2 && projectile.thrown && crit)
				damage = (int)(damage * 2.25f);
		}
	}
}
