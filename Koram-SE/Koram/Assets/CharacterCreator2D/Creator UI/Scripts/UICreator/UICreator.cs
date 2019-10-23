using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CharacterCreator2D.UI
{
    public class UICreator : MonoBehaviour
    {
        /// <summary>
        /// CharacterViewer displayed by this UICreator.
        /// </summary>
        [Tooltip("CharacterViewer displayed by this UICreator")]
        public CharacterViewer character;

        /// <summary>
        /// UIColor managed by this UICreator.
        /// </summary>
        [Tooltip("UIColor managed by this UICreator")]
        public UIColor colorUI;

        /// <summary>
        /// RuntimeDialog used by this UICreator.
        /// </summary>
        [Tooltip("RuntimeDialog used by this UICreator")]
        public RuntimeDialog dialog;

        /// <summary>
        /// JSON file location of this character in Build mode.
        /// </summary>
        [Tooltip("JSON file location of this character in build mode")]
        public string JSONRuntimePath;

        public List<GameObject> menus;
        public List<GameObject> partMenus;
        
        private bool _processing = false;

        void Awake()
        {
            _processing = false;
            BodyTypeGroup bodygroup = this.transform.GetComponentInChildren<BodyTypeGroup>(true);
            bodygroup.Initialize();
            PartGroup[] partgroups = this.transform.GetComponentsInChildren<PartGroup>(true);
            foreach (PartGroup g in partgroups)
                g.Initialize();
        }

        public void CloseAllMenus () 
        {
            if (menus != null)
            foreach (GameObject go in menus) 
            {
                go.SetActive(false);
            }
            if (partMenus != null)
            foreach (GameObject go in partMenus)
            {
                go.SetActive(false);
            }
        }

        public void OpenMenu (GameObject open)
        {
            CloseAllMenus();
            open.SetActive(true);
        }

        /// <summary>
        /// [EDITOR] Save character as a prefab in desired path.
        /// </summary>
        public void SaveCharacterAsPrefab()
        {
            if (this.character == null || _processing)
                return;

            StartCoroutine("ie_savecharaasprefab");
        }

        IEnumerator ie_savecharaasprefab()
        {
            _processing = true;
            yield return null;
#if UNITY_EDITOR
            string path = CharacterUtils.ShowSaveFileDialog("Save Character", "New Character", "prefab");
            if (!string.IsNullOrEmpty(path))
            {
                CharacterViewer tcharacter = CharacterUtils.SpawnCharacter(this.character, path);
                yield return null;
                yield return null;
                CharacterUtils.SaveCharacterToPrefab(tcharacter, path);
                yield return null;
                yield return null;
                Destroy(tcharacter.gameObject);
                dialog.DisplayDialog("Save Character", "'" + tcharacter.name + "' is saved");
            }
#endif
            _processing = false;
        }

        /// <summary>
        /// [EDITOR] Load character from a prefab.
        /// </summary>
        public void LoadCharacterFromPrefab()
        {
            _processing = true;
#if UNITY_EDITOR
            string path = CharacterUtils.ShowOpenFileDialog("Load Character", "prefab");
            if (!string.IsNullOrEmpty(path))
            {
                CharacterViewer tcharacter = CharacterUtils.LoadCharacterFromPrefab(path);
                if (tcharacter == null)
                    return;

                CharacterData data = tcharacter.GenerateCharacterData();
                this.character.AssignCharacterData(data);
                dialog.DisplayDialog("Load Character", "'" + tcharacter.name + "' is loaded");
            }
#endif
            _processing = false;
        }

        /// <summary>
        /// Save character's data as JSON file. Calling this function will save character's data with path defined in JSONRuntimePath field.
        /// </summary>
        public void SaveCharacterToJSON()
        {
            _processing = true;
            string path = "";

#if UNITY_EDITOR
            path = CharacterUtils.ShowSaveFileDialog("Save Character", "New Character Data", "json");
#else
			path = this.JSONRuntimePath;
#endif

            if (!string.IsNullOrEmpty(path))
            {
                this.character.SaveToJSON(path);
                dialog.DisplayDialog("Save Character", "'" + System.IO.Path.GetFileNameWithoutExtension(path) + "' is saved");
            }
            _processing = false;
        }

        /// <summary>
        /// Load character from JSON file's data. Calling this function will try to load character's data from a path defined in JSONRuntimePath field.
        /// </summary>
        public void LoadCharacterFromJSON()
        {
            _processing = true;
            string path = "";

#if UNITY_EDITOR
            path = CharacterUtils.ShowOpenFileDialog("Load Character", "json");
#else
			path = this.JSONRuntimePath;
#endif

            if (!string.IsNullOrEmpty(path))
            {
                this.character.LoadFromJSON(path);
                dialog.DisplayDialog("Load Character", "'" + System.IO.Path.GetFileNameWithoutExtension(path) + "' is loaded");
            }
            _processing = false;
        }

        /// <summary>
        /// Randomize character
        /// </summary>
        List<Part> armors;
        List<Part> pants;
        List<Part> helmets;
        List<Part> gloves;
        List<Part> boots;
        List<Part> capes;
        List<Part> mainhand;
        List<Part> offhand;
        List<Part> hair;
        List<Part> fhair;
        List<Part> brow;
        List<Part> eyes;
        List<Part> lips;
        List<Part> nose;
        List<Part> ears;
        List<Part> tatt;
        Part nullpart = null;
        public void RandomizePart ()
        {           
            // Get all available parts if null 
            if(armors == null)  armors = PartList.Static.FindParts(SlotCategory.Armor);
            if(pants == null)   pants = PartList.Static.FindParts(SlotCategory.Pants);
            if(helmets == null) helmets = PartList.Static.FindParts(SlotCategory.Helmet);
            if(gloves == null)  gloves = PartList.Static.FindParts(SlotCategory.Gloves);
            if(boots == null)   boots = PartList.Static.FindParts(SlotCategory.Boots);
            if(capes == null)   capes = PartList.Static.FindParts(SlotCategory.Cape);
            if(mainhand == null)mainhand = PartList.Static.FindParts(SlotCategory.MainHand);
            if(offhand == null) offhand = PartList.Static.FindParts(SlotCategory.OffHand);
            if(hair == null)    hair = PartList.Static.FindParts(SlotCategory.Hair);
            if(fhair == null)   fhair = PartList.Static.FindParts(SlotCategory.FacialHair);
            if(brow == null)    brow = PartList.Static.FindParts(SlotCategory.Eyebrow);
            if(eyes == null)    eyes = PartList.Static.FindParts(SlotCategory.Eyes);
            if(lips == null)    lips = PartList.Static.FindParts(SlotCategory.Mouth);
            if(nose == null)    nose = PartList.Static.FindParts(SlotCategory.Nose);
            if(ears == null)    ears = PartList.Static.FindParts(SlotCategory.Ear);
            if(tatt == null)    tatt = PartList.Static.FindParts(SlotCategory.SkinDetails);
            // Set body type & skin color
            if (RollDice()>50) character.SetBodyType(BodyType.Male);
            else character.SetBodyType(BodyType.Female);            
            // Equip armors
            character.EquipPart(SlotCategory.Armor, armors[Random.Range(0,armors.Count)]);
            character.EquipPart(SlotCategory.Pants, pants[Random.Range(0,pants.Count)]);
            if (RollDice()>20) character.EquipPart(SlotCategory.Helmet, helmets[Random.Range(0,helmets.Count)]);
            else character.EquipPart(SlotCategory.Helmet, nullpart);
            if (RollDice()>40) character.EquipPart(SlotCategory.Gloves, gloves[Random.Range(0,gloves.Count)]);
            else character.EquipPart(SlotCategory.Gloves, nullpart);
            character.EquipPart(SlotCategory.Boots, boots[Random.Range(0,boots.Count)]);
            if (RollDice()>60) character.EquipPart(SlotCategory.Cape, capes[Random.Range(0,capes.Count)]);
            else character.EquipPart(SlotCategory.Cape, nullpart);
            // Equip weapons
            if (RollDice()>50)
            {          
                character.EquipPart(SlotCategory.MainHand, mainhand[Random.Range(0,mainhand.Count)]);
                Weapon w = character.GetAssignedPart(SlotCategory.MainHand) as Weapon;
                if (RollDice()>50 && w.weaponCategory != WeaponCategory.TwoHanded)
                character.EquipPart(SlotCategory.OffHand, offhand[Random.Range(0,offhand.Count)]);
            }
            else
            {
                character.EquipPart(SlotCategory.MainHand, nullpart);
                character.EquipPart(SlotCategory.OffHand, nullpart);
            }
            // Equip facial feature
            if (RollDice()>10) character.EquipPart(SlotCategory.Hair, hair[Random.Range(0,hair.Count)]);
            else character.EquipPart(SlotCategory.Hair, nullpart);
            if (RollDice()>50) character.EquipPart(SlotCategory.FacialHair, fhair[Random.Range(0,fhair.Count)]);
            else character.EquipPart(SlotCategory.FacialHair, nullpart);
            if (RollDice()>2) character.EquipPart(SlotCategory.Eyebrow, brow[Random.Range(0,brow.Count)]);
            else character.EquipPart(SlotCategory.Eyebrow, nullpart);
            if (RollDice()>60) character.EquipPart(SlotCategory.Ear, ears[Random.Range(0,ears.Count)]);
            else character.EquipPart(SlotCategory.Ear, "00", "Base");
            character.EquipPart(SlotCategory.Eyes, eyes[Random.Range(0,eyes.Count)]);
            character.EquipPart(SlotCategory.Mouth, lips[Random.Range(0,lips.Count)]);
            character.EquipPart(SlotCategory.Nose, nose[Random.Range(0,nose.Count)]);
            if (RollDice()>60) character.EquipPart(SlotCategory.SkinDetails, tatt[Random.Range(0,tatt.Count)]);
            else character.EquipPart(SlotCategory.SkinDetails, nullpart);
        }
        
        public void RandomizeColor ()
        {            
            // Set Skin color
            List<Color> skins = GetColors("Skin");
            character.SkinColor = skins[Random.Range(0,skins.Count)];
            // Roll flashiness & variant
            int flashiness = RollDice();
            int variant = RollDice();
            List<Color> colorpool = new List<Color>();
            List<Color> colortheme = new List<Color>();
            List<Color> haircolor = GetColors("Hair");
            List<Color> lipscolor = GetColors("Lips");
            List<Color> tattcolor = GetColors("Grayscale");
            colorpool.AddRange(GetColors("Bleak"));
            colorpool.AddRange(GetColors("Dark"));
            colorpool.AddRange(GetColors("Leather"));
            colorpool.AddRange(GetColors("Grayscale"));
            lipscolor.AddRange(GetColors("Skin"));
            tattcolor.AddRange(GetColors("Bleak"));
            tattcolor.AddRange(GetColors("Fabric"));
            tattcolor.AddRange(GetColors("Vibrant"));
            if (flashiness >= 20) // Bleak and dark
            {                
                colorpool.AddRange(GetColors("Metal"));
                colorpool.AddRange(GetColors("Bleached"));
                haircolor.AddRange(GetColors("Metal"));
                haircolor.AddRange(GetColors("Eyes"));
                lipscolor.AddRange(GetColors("Bleached"));
            }
            if (flashiness >= 50) // Somewhat normal
            {
                colorpool.AddRange(GetColors("Fabric"));
                haircolor.AddRange(GetColors("Fabric"));
                haircolor.AddRange(GetColors("Vibrant"));
                lipscolor.AddRange(GetColors("Fabric"));
                lipscolor.AddRange(GetColors("Bleak"));
                lipscolor.AddRange(GetColors("Dark"));
            }
            if (flashiness >= 80) // Flashy af
            {
                colorpool.AddRange(GetColors("Vibrant"));
                haircolor.AddRange(GetColors("Bleached"));
                lipscolor.AddRange(GetColors("Vibrant"));
            }
            if (variant <= 30)
                for (int i = 0; i < 6; i++)
                colortheme.Add(colorpool[Random.Range(0,colorpool.Count)]);
            else if (variant > 30 && variant < 80)
                for (int i = 0; i < 9; i++)
                colortheme.Add(colorpool[Random.Range(0,colorpool.Count)]);
            else if (variant >= 80)
                for (int i = 0; i < 12; i++)
                colortheme.Add(colorpool[Random.Range(0,colorpool.Count)]);
            // Set equipment colors
            character.SetPartColor(SlotCategory.Armor, colortheme[Random.Range(0,colortheme.Count)], colortheme[Random.Range(0,colortheme.Count)], colortheme[Random.Range(0,colortheme.Count)]);
            character.SetPartColor(SlotCategory.Pants, colortheme[Random.Range(0,colortheme.Count)], colortheme[Random.Range(0,colortheme.Count)], colortheme[Random.Range(0,colortheme.Count)]);
            character.SetPartColor(SlotCategory.Helmet, colortheme[Random.Range(0,colortheme.Count)], colortheme[Random.Range(0,colortheme.Count)], colortheme[Random.Range(0,colortheme.Count)]);
            character.SetPartColor(SlotCategory.Gloves, colortheme[Random.Range(0,colortheme.Count)], colortheme[Random.Range(0,colortheme.Count)], colortheme[Random.Range(0,colortheme.Count)]);
            character.SetPartColor(SlotCategory.Boots, colortheme[Random.Range(0,colortheme.Count)], colortheme[Random.Range(0,colortheme.Count)], colortheme[Random.Range(0,colortheme.Count)]);
            character.SetPartColor(SlotCategory.Cape, colortheme[Random.Range(0,colortheme.Count)], colortheme[Random.Range(0,colortheme.Count)], colortheme[Random.Range(0,colortheme.Count)]);
            character.SetPartColor(SlotCategory.MainHand, colorpool[Random.Range(0,colorpool.Count)], colorpool[Random.Range(0,colorpool.Count)], colorpool[Random.Range(0,colorpool.Count)]);
            character.SetPartColor(SlotCategory.OffHand, colorpool[Random.Range(0,colorpool.Count)], colorpool[Random.Range(0,colorpool.Count)], colorpool[Random.Range(0,colorpool.Count)]);
            // Set face colors
            Color newcolor = haircolor[Random.Range(0,haircolor.Count)];
            character.SetPartColor(SlotCategory.Hair, ColorCode.Color1, newcolor);
            character.SetPartColor(SlotCategory.FacialHair, ColorCode.Color1, newcolor);
            if (flashiness < 80)
            character.SetPartColor(SlotCategory.Eyebrow, ColorCode.Color1, newcolor);
            else            
            character.SetPartColor(SlotCategory.Eyebrow, ColorCode.Color1, haircolor[Random.Range(0,haircolor.Count)]);
            List<Color> eyecolor = GetColors("Eyes");
            if (RollDice() > 60)
            eyecolor.AddRange(GetColors("Vibrant"));
            character.SetPartColor(SlotCategory.Eyes, ColorCode.Color1, eyecolor[Random.Range(0,eyecolor.Count)]);
            if (character.bodyType == BodyType.Female)            
            character.SetPartColor(SlotCategory.Mouth, ColorCode.Color1, lipscolor[Random.Range(0,lipscolor.Count)]);
            else
            character.SetPartColor(SlotCategory.Mouth, ColorCode.Color1, character.SkinColor);
            if (character.GetAssignedPart(SlotCategory.SkinDetails) != null)
            {
                newcolor = tattcolor[Random.Range(0,tattcolor.Count)];
                if(RollDice()>50) newcolor.a = Random.Range(0.2f,1f);
            }
            else newcolor = Color.gray;
            character.SetPartColor(SlotCategory.SkinDetails, ColorCode.Color1, newcolor);
        }

        int RollDice ()
        {
            int i = Random.Range(0,100);
            return i;
        }

        List<Color> GetColors (string palette)
        {
            List<Color> colors = new List<Color>();
            foreach (ColorPalette p in this.colorUI.colorPalette.colorPalettes)
            {
                if (p.paletteName == palette)
                {
                    foreach (Color c in p.colors) colors.Add(c);
                    break;
                }
            }
            return colors;
        }
    }
}