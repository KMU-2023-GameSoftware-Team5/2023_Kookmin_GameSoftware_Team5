using data;
using lee;
using placement;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace deck
{
    /// <summary>
    /// 筌?Ŧ????醫뤾문 ?온??揶쏆빘猿?
    /// </summary>
    /// <remarks>
    /// 筌?Ŧ????醫뤾문 ??肉??筌?Ŧ????온????끦®몴?筌ｌ꼶??? ?곕???筌?Ŧ????袁⑥뺘?怨몄뵥 ?온?귐됱쨮 ?類ㅼ삢?醫?????깆벥 ?袁⑹뒄
    /// </remarks>
    public class CharacterSelectManager : MonoBehaviour
    {
        private static CharacterSelectManager instance;
        public static CharacterSelectManager Instance()
        {
            if (instance == null)
            {
                instance = FindObjectOfType<CharacterSelectManager>();
            }
            return instance;             
        }

        /// <summary>
        /// ?袁⑹삺 ???쟿??곷선揶쎛 揶쎛筌왖????덈뮉 筌?Ŧ??怨쀬벥 筌욌쵑鍮
        /// </summary>
        public List<PixelCharacter> characters;
        /// <summary>
        /// ?袁⑹삺 ???쟿??곷선揶쎛 ?醫뤾문??筌?Ŧ??怨쀬벥 筌욌쵑鍮
        /// </summary>
        public PixelCharacter[] selectCharacters;

        /// <summary>
        /// 筌?Ŧ????醫뤾문 ?????????獄쏄퀣肉?
        /// </summary>
        public CharacterSelector[] selectors;

        /// <summary>
        /// 筌?Ŧ????醫뤾문 ??源??筌ｌ꼶?곭몴??袁る립 ?袁⑸뻻癰궰??
        /// </summary>
        public int nowSelectorId;
        
        /// <summary>
        /// Drag ??源??筌ｌ꼶?곭몴??袁る립 canvas??딅쓠?怨쀫뮞
        /// </summary>
        [SerializeField]
        GameObject selectUICanvas;

        [Header("Character List")]
        /// <summary>
        /// ???쟿??곷선揶쎛 ?袁⑹삺 揶쎛筌왖????덈뮉 筌?Ŧ??怨뺣굶??癰귣똻肉т틠?곕뮉 ?귐딅뮞?紐꾩벥 ?袁⑺뒄
        /// </summary>
        [SerializeField]
        Transform characterInventoryGrid;
        /// <summary>
        /// ???쟿??곷선揶쎛 ?袁⑹삺 揶쎛筌왖????덈뮉 筌?Ŧ??怨쀬젟癰??袁ⓥ봺??
        /// </summary>
        [SerializeField]
        GameObject characterInventoryItemPrefeb;

        [Header("Character Selector UI")]
        /// <summary>
        /// 筌?Ŧ????醫뤾문筌??袁⑺뒄
        /// </summary>
        [SerializeField]
        Transform characterSelectorGrid;
        /// <summary>
        /// 筌?Ŧ????醫뤾문 ??????????袁ⓥ봺??
        /// </summary>
        [SerializeField]
        GameObject characterSelectorPrefeb;

        [Header("Character Details")]
        /// <summary>
        /// 筌?Ŧ????紐? ?類ｋ궖筌?UI
        /// </summary>
        [SerializeField]
        CharacterDetails characterDetails;

        /// <summary>
        /// 獄쏄퀣??UI 獄쏄퀣肉?
        /// /summary>
        PlacementCharacter[] placementUIs;

        /// <summary>
        /// 筌?Ŧ???獄쏄퀣??筌뤴뫀諭? TODO
        /// </summary>
        public bool isPlacementMode { get; private set; }

        [Header("Character Placement")]
        /// <summary>
        /// 임시속성 - 캐릭터 배치 오브젝트의 parent canvas
        /// </summary>
        public Transform placementCanvas;

        /// <summary>
        /// placement Character(캐릭터 배치 오브젝트) 프리펩
        /// </summary>
        [SerializeField] GameObject placementCharacterPrefab;

        [Header("Character Placement Head Bar")]
        /// <summary>
        /// Placement - 캐릭터 헤드 네임이 위치할 캔버스
        /// </summary>
        public Transform characterHeadNameCanvas;
        /// <summary>
        /// Placement - 배치시 캐릭터 위에 이름 달아줄 프리펩
        /// </summary>
        [SerializeField] GameObject characterHeadNamePrefab;

        lee.PixelCharacter[] battlePixelCharacters;

        void Start()
        {
            isPlacementMode = false;

<<<<<<< Updated upstream
            // 플레이어매니저에게서 보유 캐릭터 받아오기
            characters = PlayerManager.Instance().playerCharacters;


            // 더미데이터 생성
            if(characters.Count == 0)
            {
                string[] characterNames = { "Demon", "Skeleton", "Goblin Archor" };
                System.Random random = new System.Random();
                for (int i = 0; i < 7; i++)
                {
                    PlayerManager.Instance().addCharacterByName(characterNames[random.Next(0, characterNames.Length)]);
                }
            }

            // 현재 보유중인 캐릭터 출력
            for (int i = 0; i < characters.Count; i++)
=======
            // ?袁⑸뻻 ?怨쀬뵠????밴쉐
            characters = new PixelCharacter[6];
            
            characters[0] = MyDeckFactory.Instance().buildPixelCharacter("blue");
            characters[1] = MyDeckFactory.Instance().buildPixelCharacter("magenta");
            characters[2] = MyDeckFactory.Instance().buildPixelCharacter("cyan");
            characters[3] = MyDeckFactory.Instance().buildPixelCharacter("yellow");
            characters[4] = MyDeckFactory.Instance().buildPixelCharacter("red");
            characters[5] = MyDeckFactory.Instance().buildPixelCharacter("green");

            // ?袁⑹삺 癰귣똻?餓λ쵐??筌?Ŧ????곗뮆??
            for (int i = 0; i < characters.Length; i++)
>>>>>>> Stashed changes
            {
                createCharacterInventoryPrefeb(i, characters[i]); 
            }

            // 筌?Ŧ????醫뤾문 ??????밴쉐
            selectCharacters = new PixelCharacter[5]; 
            selectors = new CharacterSelector[5];
            for(int i=0; i < selectors.Length; i++)
            {
                selectors[i] = createCharacterSelectorPrefeb(i);

            }

            placementUIs = new PlacementCharacter[5];
            battlePixelCharacters = new lee.PixelCharacter[5];
        }

        /// <summary>
        /// ???쟿??곷선揶쎛 ?袁⑹삺 癰귣똻? 餓λ쵐??筌?Ŧ????類ｋ궖 UI ??밴쉐
        /// </summary>
<<<<<<< Updated upstream
        /// <param name="i">TODO - 추후 제거 필요(미사용)</param>
        /// <param name="character">플레이어의 캐릭터 정보</param>
=======
        /// <param name="i">?곕?????볤탢 ?袁⑹뒄(沃섎챷沅??</param>
        /// <param name="character">???쟿??곷선??筌?Ŧ????類ｋ궖</param>
>>>>>>> Stashed changes
        void createCharacterInventoryPrefeb(int i, PixelCharacter character)
        {
            GameObject newPrefab = Instantiate(characterInventoryItemPrefeb, characterInventoryGrid);
            newPrefab.GetComponent<CharacterListItem>().Initialize(character, selectUICanvas.transform, characterInventoryGrid.transform);
        }

        /// <summary>
        /// 筌?Ŧ????醫뤾문 ??????袁⑺뒄
        /// </summary>
        /// <param name="selectId">筌?Ŧ????醫뤾문 ??????⑥쥙? ID</param>
        /// <returns></returns>
        CharacterSelector createCharacterSelectorPrefeb(int selectId)
        {
            GameObject newPrefab = Instantiate(characterSelectorPrefeb, characterSelectorGrid);
            CharacterSelector selector = newPrefab.GetComponent<CharacterSelector>();
            selector.Initialize(selectId);
            return selector; 
        }

        /// <summary>
        /// 筌?Ŧ????紐? ?類ｋ궖筌???용┛
        /// </summary>
        /// <param name="character">?紐? ?類ｋ궖筌≪럩????곷선??노릭??筌?Ŧ???/param>
        public void openCharacterDetails(PixelCharacter character)
        {
            characterDetails.openCharacterDetails(character);
        }

        /// <summary>
        /// 筌?Ŧ????醫뤾문 筌ｌ꼶??筌롫뗄苑??
        /// </summary>
        /// <param name="selectId">筌?Ŧ??怨? 筌뤿돃苡???醫뤾문??뤿??遺?</param>
        /// <param name="character">?醫뤾문??筌?Ŧ????類ｋ궖</param>
        public void selectCharacter(int selectId, PixelCharacter character)
        {
            selectCharacters[selectId] = character;
            placementUIs[selectId] = buildPixelHumanoidByPixelCharacter(
                (PixelHumanoid)selectCharacters[selectId]
            );
            battlePixelCharacters[selectId] =  placementUIs[selectId].GetComponent<lee.PixelCharacter>();
        }

        /// <summary>
        /// 筌?Ŧ????醫뤾문??곸젫 ??源??
        /// </summary>
        /// <param name="selectId">?醫뤾문??곸젫??筌?Ŧ??怨? 筌뤿돃苡???????醫뤾문??筌?Ŧ??怨쀬뵥揶쎛</param>
        public void unSelectCharacter(int selectId)
        {
            selectCharacters[selectId] = null;
            battlePixelCharacters[selectId] = null;
            placementUIs[selectId].unSelect();
            placementUIs[selectId] = null;
            //logSelectors();
        }

        /// <summary>
        /// 筌?Ŧ??怨쀫퓠野??袁⑹뵠???關媛???源??
        /// </summary>
        /// <param name="character">?袁⑹뵠??뽰뱽 ?關媛??筌?Ŧ???/param>
        /// <param name="equipId">筌?Ŧ??怨? 筌뤿돃苡??紐껉뭣?醫듼봺???袁⑹뵠??뽰뱽 ?關媛??野껉퍔?ㅿ쭪?</param>
        /// <param name="item">?關媛???袁⑹뵠??/param>
        public bool equip(PixelCharacter character, int equipId, EquipItem item)
        {
            return character.equip(equipId, item);
        }

        /// <summary>
        /// 筌?Ŧ????袁⑹뵠???關媛???곸젫 ??源??
        /// </summary>
        /// <param name="character">?袁⑹뵠??뽰뱽 ??곸젫??筌?Ŧ???/param>
        /// <param name="equipId">??곸젫???袁⑹뵠??뽰벥 ?紐껉뭣?醫듼봺???袁⑺뒄</param>
        public bool unEquip(PixelCharacter character, int equipId)
        {
            return character.unEquip(equipId);
        }

        /// <summary>
        /// 筌?Ŧ???獄쏄퀣??筌뤴뫀諭뜻에?筌띾슢諭얏묾? TODO
        /// </summary>
        public void togglePlacementMode()
        {
            isPlacementMode = isPlacementMode ? false : true;
            selectUICanvas.SetActive(!isPlacementMode);
            foreach(PlacementCharacter ui in placementUIs)
            {
                if(ui != null)
                {
                    ui.dragMode = isPlacementMode;
                }
            }
        }

        /// <summary>
        /// 배치할 캐릭터를 생성하는 메서드
        /// </summary>
        /// <param name="character">캐릭터 셀렉트 매니저가 가진 캐릭터 정보</param>
        /// <returns>캐릭터셀렉트매니저가 관리할 수 있는 캐릭터 배치 컴포넌트</returns>
        public PlacementCharacter buildPixelHumanoidByPixelCharacter(deck.PixelHumanoid character)
        {
            // Instantiate pixel humanoid
            Vector3 worldPosition = character.worldPosition;
            GameObject characterGo = Instantiate(placementCharacterPrefab, Vector3.zero, Quaternion.identity, placementCanvas);
            characterGo.transform.position = worldPosition;

            // build battle.PixelHumaniod
            lee.PixelHumanoid battlPixelHumanoid = characterGo.GetComponent<lee.PixelHumanoid>();
            battlPixelHumanoid.builder.SpriteCollection = StaticLoader.Instance().GetCollection();
            battlPixelHumanoid.builder.SpriteLibrary = battlPixelHumanoid.spriteLibrary;
            PixelHumanoidData data = MyDeckFactory.Instance().getPixelHumanoidData(character.characterName);
            data.SetOutToBuilder(battlPixelHumanoid.builder);
            battlPixelHumanoid.builder.Rebuild();
            battlPixelHumanoid.Initilize(data);

            // PlacementCharacter build
            PlacementCharacter ret = characterGo.GetComponent<PlacementCharacter>();
            ret.Initialize(character);

            // Instantiate head Bar
            GameObject headBarGo = Instantiate(characterHeadNamePrefab, Vector3.zero, Quaternion.identity, characterHeadNameCanvas);

            // head bar setting
            PixelCharacterHeadBar headBar = headBarGo.GetComponent<PixelCharacterHeadBar>();
            headBar.Initialize(battlPixelHumanoid);
            battlPixelHumanoid.headBar = headBar;

            // head name setting
            PlacementCharacterHeadName headName = headBarGo.GetComponent<PlacementCharacterHeadName>();
            headName.Initialize(character.characterNickName);
            ret.headName = headName;

            return ret;
        }

        public lee.PixelCharacter[] battleStart()
        {
            foreach(PlacementCharacter target in placementUIs)
            {
                if(target != null)
                {
                    target.battleStart();
                }
            }

            return battlePixelCharacters;
        }
    }
}
