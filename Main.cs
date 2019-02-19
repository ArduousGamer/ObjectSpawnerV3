//Object Spawner v3 by ArduousGamer aka DementedDude
//Message Board provided by @ https://forum.fivem.net/t/release-message-board-road-work-ahead/263849
//Object Spawning and Deleting by the best FiveM Dev: Xander1998
//MenuAPI by @ https://forum.fivem.net/t/c-menuapi-mapi-v1-1-2/204992
//Coroner stretcher provided by @ http://www.lcpdfr.com/files/file/18046-body-bags/
//Object names provided by @ https://objects.gt-mp.net/index.php

using System;
using System.Threading.Tasks;
using CitizenFX.Core;
using CitizenFX.Core.UI;
using static CitizenFX.Core.Native.API;
using CitizenFX.Core.Native;
using MenuAPI;
using System.Collections.Generic;

namespace ObjectSpawnverv3
{
    public class Main : BaseScript
    {

        private static Vector3 OffsetPlacementPosition = new Vector3(0f, 1f, 100f);
        public static List<Prop> SpawnedProps = new List<Prop>();

        public Main()
        {
            Tick += OnTick;
        }

        public static async void SpawnObject(string model)
        {
            Vector3 SpawnPos = Game.Player.Character.GetOffsetPosition(OffsetPlacementPosition);
            float CurrentHeading = Game.Player.Character.Heading;
            Prop CreatedProp = await World.CreateProp(new Model(model), SpawnPos, false, true);
            CreatedProp.Position = new Vector3(CreatedProp.Position.X, CreatedProp.Position.Y, CreatedProp.Position.Z);
            CreatedProp.Heading = CurrentHeading;
            CreatedProp.IsInvincible = true;
            CreatedProp.IsPositionFrozen = true;
            CreatedProp.IsCollisionProof = true;
            CreatedProp.IsBulletProof = true;
            CreatedProp.IsMeleeProof = true;
            SpawnedProps.Add(CreatedProp);
        }

        public static void PickupLastObject()
        {
            if (SpawnedProps.Count >= 1)
            {
                int DeleteIndex = SpawnedProps.Count - 1;
                SpawnedProps[DeleteIndex].Delete();
                SpawnedProps.RemoveAt(DeleteIndex);
                CitizenFX.Core.UI.Screen.ShowNotification("~b~Object Spawner~n~~y~Last Object Removed");
            }
        }

        public static void PickupAllObjects()
        {
            if (SpawnedProps.Count >= 1)
            {
                foreach (Prop p in SpawnedProps)
                {
                    p.Delete();
                }
                SpawnedProps.Clear();
                CitizenFX.Core.UI.Screen.ShowNotification("~b~Object Spawner~n~~y~All Objects Removed");
            }
        }

        private async Task OnTick()
        {
            await Task.FromResult(0);

            if (IsControlJustReleased(1, 167))
            {

                if (MenuController.IsAnyMenuOpen())
                {
                    MenuController.CloseAllMenus();
                }
                else
                {
                    MenuController.MenuToggleKey = (Control)(-1);
                    CreateObjectsMenu();
                }
            }

        }

        public void CreateObjectsMenu()
        {
            MenuController.MenuAlignment = MenuController.MenuAlignmentOption.Left;
            Menu objMenu = new Menu("Object Spawner", "Version 3.0") { Visible = true };
            MenuController.AddMenu(objMenu);
            MenuItem CD = new MenuItem("Channelizer Drum"); objMenu.AddMenuItem(CD);
            MenuItem CS = new MenuItem("Coroner Stretcher"); objMenu.AddMenuItem(CS);
            MenuItem MB = new MenuItem("Message Board"); objMenu.AddMenuItem(MB);
            MenuItem PB = new MenuItem("Police Barrier"); objMenu.AddMenuItem(PB);
            MenuItem TC = new MenuItem("Traffic Cone"); objMenu.AddMenuItem(TC);
            MenuItem WB = new MenuItem("Work Barrier"); objMenu.AddMenuItem(WB);
            MenuItem WBA = new MenuItem("Work Barrier (Arrow)"); objMenu.AddMenuItem(WBA);            
            MenuItem DLO = new MenuItem("Delete Last Object"); objMenu.AddMenuItem(DLO);
            MenuItem DAO = new MenuItem("Delete All Objects"); objMenu.AddMenuItem(DAO);
            MenuItem EM = new MenuItem("Exit Menu"); objMenu.AddMenuItem(EM);

            objMenu.OnItemSelect += (_menu, _item, _index) =>
            {

                if (_item == CD)
                {
                    SpawnObject("prop_barrier_wat_03a");
                }
                else if (_item == PB)
                {
                    SpawnObject("prop_barrier_work05");
                }
                else if (_item == TC)
                {
                   SpawnObject("prop_mp_cone_01");
                }
                else if (_item == WB)
                {
                    SpawnObject("prop_mp_barrier_02b");
                }
                else if (_item == WBA)
                {
                    SpawnObject("prop_mp_arrow_barrier_01");
                }
                else if (_item == CS)
                {
                    SpawnObject("prop_ld_binbag_01");
                }
                else if (_item == MB)
                {
                    SpawnObject("prop_trafficdiv_01");
                }
                else if (_item == DLO)
                {
                    PickupLastObject();
                }
                else if (_item == DAO)
                {
                    PickupAllObjects();
                }
                else if (_item == EM)
                {
                    MenuController.CloseAllMenus();
                }

            };
        }

    }
}
