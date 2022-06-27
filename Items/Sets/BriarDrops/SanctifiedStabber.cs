using SpiritMod.Buffs;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
namespace SpiritMod.Items.Sets.BriarDrops
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
			Item.useStyle = ItemUseStyleID.Thrust;
			Item.useTurn = false;
			Item.useAnimation = 6;
			Item.useTime = 18;
			Item.width = 24;
			Item.rare = ItemRarityID.Blue;
			Item.height = 28;
			Item.damage = 11;
			Item.knockBack = 3f;
			Item.scale = 0.9f;
			Item.UseSound = SoundID.Item1;
			Item.useTurn = true;
			Item.value = 3000;
			Item.DamageType = DamageClass.Melee;
		}
		public override void OnHitNPC(Player player, NPC target, int damage, float knockback, bool crit)
		{
			if (Main.rand.Next(2) == 0) {
				target.AddBuff(ModContent.BuffType<WitheringLeaf>(), 180);
			}
		}
	}
}