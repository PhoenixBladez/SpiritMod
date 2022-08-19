using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.BossLoot.MoonWizardDrops.MJWPet
{
	internal class MJWPetItem : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Curious Lightbulb");
			Tooltip.SetDefault("Summons a Moon Jelly Lightbulb");
		}

		public override void SetDefaults()
		{
			Item.CloneDefaults(ItemID.ShadowOrb);
			Item.shoot = ModContent.ProjectileType<MJWPetProjectile>();
			Item.buffType = ModContent.BuffType<Buffs.Pet.MJWPetBuff>();
			Item.UseSound = SoundID.NPCDeath6; 
			Item.rare = ItemRarityID.Master;
			Item.master = true;
		}

		public override void UseStyle(Player player, Rectangle heldItemFrame)
		{
			if (player.whoAmI == Main.myPlayer && player.itemTime == 0)
				player.AddBuff(Item.buffType, 3600, true);
		}

		public override bool CanUseItem(Player player) => player.miscEquips[1].IsAir;
	}
}
