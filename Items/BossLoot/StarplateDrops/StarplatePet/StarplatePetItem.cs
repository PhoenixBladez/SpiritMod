using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.BossLoot.StarplateDrops.StarplatePet
{
	internal class StarplatePetItem : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Starplate Controller");
			Tooltip.SetDefault("Summons a Starplate miniature");
		}

		public override void SetDefaults()
		{
			Item.CloneDefaults(ItemID.Fish);
			Item.shoot = ModContent.ProjectileType<StarplatePetProjectile>();
			Item.buffType = ModContent.BuffType<Buffs.Pet.StarplatePetBuff>();
			Item.UseSound = SoundID.NPCDeath6; 
			Item.rare = ItemRarityID.Master;
			Item.master = true;
		}

		public override void UseStyle(Player player, Rectangle heldItemFrame)
		{
			if (player.whoAmI == Main.myPlayer && player.itemTime == 0)
				player.AddBuff(Item.buffType, 3600, true);
		}

		public override bool? UseItem(Player player) => base.UseItem(player);

		public override bool CanUseItem(Player player) => player.miscEquips[0].IsAir;
	}
}
