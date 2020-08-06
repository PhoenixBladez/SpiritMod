using SpiritMod.Buffs;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
namespace SpiritMod.Items.Weapon.Spear
{
	public class SanctifiedStabber : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Sanctified Stabber");
			Tooltip.SetDefault("Inflicts 'Withering Leaf'");
		}


		public override void SetDefaults()
		{
			item.useStyle = ItemUseStyleID.Stabbing;
			item.useTurn = false;
			item.useAnimation = 6;
			item.useTime = 18;
			item.width = 24;
			item.rare = ItemRarityID.Blue;
			item.height = 28;
			item.damage = 11;
			item.knockBack = 3f;
			item.scale = 0.9f;
			item.UseSound = SoundID.Item1;
			item.useTurn = true;
			item.value = 3000;
			item.melee = true;
		}
		public override void OnHitNPC(Player player, NPC target, int damage, float knockback, bool crit)
		{
			if (Main.rand.Next(2) == 0) {
				target.AddBuff(ModContent.BuffType<WitheringLeaf>(), 180);
			}
		}
	}
}