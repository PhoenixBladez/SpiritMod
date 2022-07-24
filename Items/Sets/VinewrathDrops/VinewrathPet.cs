using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Sets.VinewrathDrops
{
	internal class VinewrathPet : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Wrathful Seedling");
			Tooltip.SetDefault("Summons an angry Seedling companion");
		}

		public override void SetDefaults()
		{
			Item.CloneDefaults(ItemID.Fish);
			Item.shoot = ModContent.ProjectileType<VinewrathPetProjectile>();
			Item.buffType = 0;
			//Item.buffTime = 50;
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
