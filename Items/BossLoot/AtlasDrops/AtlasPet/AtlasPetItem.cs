using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.BossLoot.AtlasDrops.AtlasPet
{
	internal class AtlasPetItem : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Primordial Pebble");
			Tooltip.SetDefault("Summons Atlas Jr.");
		}

		public override void SetDefaults()
		{
			Item.CloneDefaults(ItemID.Fish);
			Item.shoot = ModContent.ProjectileType<AtlasPetProjectile>();
			Item.buffType = ModContent.BuffType<Buffs.Pet.AtlasPetBuff>();
			Item.UseSound = SoundID.NPCDeath6; 
			Item.rare = ItemRarityID.Master;
			Item.master = true;
		}

		public override void UseStyle(Player player, Rectangle heldItemFrame)
		{
			if (player.whoAmI == Main.myPlayer && player.itemTime == 0)
				player.AddBuff(Item.buffType, 3600, true);
		}

		public override bool CanUseItem(Player player) => player.miscEquips[0].IsAir;
	}
}
