using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Linq;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using SpiritMod.Items.GamblerChestLoot.Jem;
using SpiritMod.Items.GamblerChestLoot.FunnyFirework;
using SpiritMod.Items.GamblerChestLoot.GildedMustache;
using SpiritMod.Items.GamblerChestLoot.Champagne;
using SpiritMod.Mechanics.Fathomless_Chest;
using SpiritMod.Items.GamblerChestLoot.RegalCane;

namespace SpiritMod.Items.Consumable.GamblerChests.GamblerChestNPCs
{
    internal class CopperChestBottom : ModNPC
    {
        public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Copper Chest");
            Main.npcFrameCount[npc.type] = 4;
        }
        public override void SetDefaults()
        {
            npc.width = 30;
            npc.height = 26;
            npc.knockBackResist = 0;
            npc.aiStyle = -1;
            npc.lifeMax = 1;
            npc.immortal = true;
            npc.noTileCollide = false;
            npc.dontCountMe = true;
        }
        int counter = -1;
        float alphaCounter = 0;

        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
             Vector2 center = new Vector2((float)(Main.npcTexture[npc.type].Width / 2), (float)(Main.npcTexture[npc.type].Height / Main.npcFrameCount[npc.type] / 2));
             Rectangle rect = new Rectangle(0, frame * (Main.npcTexture[npc.type].Height / 4), Main.npcTexture[npc.type].Width, (Main.npcTexture[npc.type].Height / 4));
            Main.spriteBatch.Draw(mod.GetTexture("Items/Consumable/GamblerChests/GamblerChestNPCs/CopperChestTop"), (npc.Center - Main.screenPosition + new Vector2(0,4)), rect, lightColor, npc.rotation, center, npc.scale, SpriteEffects.None, 0f);
            if (counter > 0 && frame > 0)
            {
                Main.spriteBatch.Draw(mod.GetTexture("Effects/Masks/Extra_49_Top"), (npc.Center - Main.screenPosition) + new Vector2(-2, 4), null, new Color(200, 200, 200, 0), 0f, new Vector2(50, 50), 0.3f * npc.scale, SpriteEffects.None, 0f);
            }
            Main.spriteBatch.Draw(Main.npcTexture[npc.type], (npc.Center - Main.screenPosition + new Vector2(0,4)), rect, lightColor, npc.rotation, center, npc.scale, SpriteEffects.None, 0f);
            return false;
        }
        bool rightClicked = false;
        int frame = 0;
        public override void AI()
        {
            if (!rightClicked && npc.velocity.Y == 0 && counter < -70)
            {
                rightClicked = true;
            }
            if (rightClicked && npc.velocity.Y != 0)
            {
                npc.rotation += Main.rand.NextFloat(-0.1f,0.1f);
            }
            counter--;
            if (counter == 0)
            {
                Gore.NewGore(npc.position, npc.velocity, 11);
				Gore.NewGore(npc.position, npc.velocity, 12);
                Gore.NewGore(npc.position, npc.velocity, 13);
				Main.PlaySound(SoundID.DoubleJump, npc.Center);
				npc.active = false;
               // Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/XynonCrateGore_2"), 1f);
               // Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/XynonCrateGore_3"), 1f);
               // Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/XynonCrateGore_4"), 1f);
            }
            if (counter % 10 == 0)
            {
               int dust = Dust.NewDust(npc.position, npc.width, npc.height, 244, 0, 0);
               Main.dust[dust].velocity = Vector2.Zero;
            }
            if (counter > 0)
            {
                 if (counter < 14)
                {
                    npc.scale = counter / 14f;
                }
                if (frame < 3 && counter % 12 == 4)
                {
                    frame++;
                }
                if (counter <= 50 && counter % 10 == 0)
                {
                    int itemid;
                    int item = 0;
                    float val = Main.rand.NextFloat();
                    if (val < .454f)
                    {
                        itemid = ItemID.CopperCoin;
                    }
                    else if (val < 0.99f)
                    {
                        itemid = ItemID.SilverCoin;
                    }
                    else
                    {
                        itemid = ItemID.GoldCoin;
                    }

                    item = Item.NewItem(npc.Center, Vector2.Zero, itemid, 1);
                    Main.item[item].velocity = Vector2.UnitY.RotatedBy(Main.rand.NextFloat(1.57f, 4.71f)) * 4;
                    Main.item[item].velocity.Y /= 2;
                }
				if (counter == 25) {
					npc.DropItem(ModContent.ItemType<Jem>(), 0.0025f);
                    npc.DropItem(ModContent.ItemType<Items.Consumable.Food.GoldenCaviar>(), 0.05f);
					npc.DropItem(ModContent.ItemType<FunnyFirework>(), 0.05f, Main.rand.Next(5, 9));
					npc.DropItem(ItemID.AngelStatue, 0.05f);
					npc.DropItem(ModContent.ItemType<Champagne>(), 0.04f, Main.rand.Next(1, 3));
                    npc.DropItem(ModContent.ItemType<Mystical_Dice>(), 0.01f);
					switch (Main.rand.NextBool()) { //mutually exclusive
						case true:
							npc.DropItem(ModContent.ItemType<GildedMustache>(), 0.01f);
							break;
						case false:
							npc.DropItem(ModContent.ItemType<RegalCane>(), 0.01f);
							break;
					}
				}
            }
        }
        public override void FindFrame(int frameHeight)
		{
                if (rightClicked && npc.velocity.Y == 0 && counter < 0)
                {
                    npc.rotation = 0;
                    npc.frame.Y = frameHeight;
                    counter = 100;
                    //Projectile.NewProjectile(npc.Center, Vector2.Zero, ModContent.ProjectileType<CopperChestTop>(), 0, 0, npc.target, npc.position.X, npc.position.Y);
                }
        }
    }
}
