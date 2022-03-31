using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace SpiritMod.Items.Armor.StarjinxSet
{
	[AutoloadEquip(EquipType.Head)]
    public class StarlightHat : ModItem
	{
		public override bool Autoload(ref string name) => false;

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Starlight Hat");
			Tooltip.SetDefault("12% increased magic damage and 6% increased magic critical strike chance");
			SpiritGlowmask.AddGlowMask(item.type, Texture + "_glow");
		}
		public override void SetDefaults()
        {
            item.width = 30;
            item.height = 24;
            item.value = Item.sellPrice(gold : 8);
            item.rare = ItemRarityID.Pink;
            item.defense = 7;
		}

		public override void DrawHair(ref bool drawHair, ref bool drawAltHair) => drawHair = drawAltHair = false;
		public override bool DrawHead() => false;

		public override void UpdateEquip(Player player)
		{
			player.magicDamage += 0.12f;
			player.magicCrit += 6;
		}
		public override void DrawArmorColor(Player drawPlayer, float shadow, ref Color color, ref int glowMask, ref Color glowMaskColor) => glowMaskColor = Color.White * 0.75f;
		public override bool IsArmorSet(Item head, Item body, Item legs) => body.type == mod.ItemType("StarlightMantle") && legs.type == mod.ItemType("StarlightSandals");

		public override void UpdateArmorSet(Player player)
        {
			player.setBonus = ("Greatly increases mana useage and prevents useage of mana potions\n"
					  + "Running out of mana produces a manajinx pylon near you\n"
					  + "Collecting the manajinx pylon restores all mana and temporarily enchants magic weapons with stars");
			player.manaCost *= 1.5f;
			MyPlayer modplayer = (MyPlayer)player.GetModPlayer(mod, "MyPlayer");
			modplayer.StarjinxSet = true;
			if (Main.rand.Next(30) == 0)
		    {
				int gore = Gore.NewGore(player.position + new Vector2(Main.rand.Next(player.width), Main.rand.Next(player.height)), 
					player.velocity / 2 + Main.rand.NextVector2Circular(1, 1), 
					mod.GetGoreSlot("Gores/StarjinxGore"), 
					Main.rand.NextFloat(0.25f, 0.75f));
				Main.playerDrawGore.Add(gore);
			}
		}
		public override void ArmorSetShadows(Player player) => player.armorEffectDrawOutlinesForbidden = true;
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(mod, "Starjinx", 8);
			recipe.AddIngredient(ItemID.Silk, 4);
			recipe.AddIngredient(ItemID.FallenStar, 5);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
    }
}
