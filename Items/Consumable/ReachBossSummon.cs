using SpiritMod.NPCs.Boss.ReachBoss;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Consumable
{
    public class ReachBossSummon : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Bramble Tooth");
            Tooltip.SetDefault("'A malevolent mixture of flora and fauna'\nUse in the Underground Briar to summon the Vinewrath Bane");
        }

        public override void SetDefaults()
        {
            Item.width = Item.height = 16;
            Item.rare = ItemRarityID.Green;
            Item.maxStack = 99;

            Item.useStyle = ItemUseStyleID.HoldUp;
            Item.useTime = Item.useAnimation = 20;

            Item.noMelee = true;
            Item.consumable = true;
            Item.autoReuse = false;

            Item.UseSound = SoundID.Item43;
        }

		public override bool CanUseItem(Player player) => !NPC.AnyNPCs(ModContent.NPCType<ReachBoss>()) && player.GetSpiritPlayer().ZoneReach && !player.ZoneOverworldHeight;

		public override bool? UseItem(Player player)
        {
            NPC.SpawnOnPlayer(player.whoAmI, ModContent.NPCType<ReachBoss>());
            SoundEngine.PlaySound(SoundID.Roar, player.position);
            return true;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(null, "BismiteCrystal", 2);
            recipe.AddIngredient(null, "EnchantedLeaf", 2);
            recipe.AddRecipeGroup("SpiritMod:PHMEvilMaterial", 2);
            recipe.AddTile(TileID.Anvils);
            recipe.Register();
        }
    }
}