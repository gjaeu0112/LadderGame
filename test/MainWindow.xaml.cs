using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace test
{
    /// <summary>
    /// MainWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            GridButton.Click += GridButton_Click;
        }
        class Item
        {
            public int id;
            public string name;
            public int damage;

            public Item(int id, string name, int damage)
            {
                this.id = id;
                this.name = name;
                this.damage = damage;
            }
        }
        private void GridButton_Click(object sender, RoutedEventArgs e)
        {
            //테스트를 위한 데이터 생성.
            Item[] itemArr = {
                new Item(0, "장검", 10),
                new Item(1, "단검", 8),
                new Item(2, "활", 12),
                new Item(3, "총", 15),
                new Item(4, "도끼", 20)
            };

            //테스트를 위한 데이터 삽입.
            var itemDIct = new Dictionary<int, Item>();
            var itemList = new List<Item>();

            foreach (var item in itemArr)
            {
                itemDIct.Add(item.id, item);
                itemList.Add(item);
            }

            //where 사용.

            //itemDict에서 데미지가 12미만인 아이템 가져오기. 
            IEnumerable<KeyValuePair<int, Item>> items1 = itemDIct.Where(x => x.Value.damage < 12);
            foreach (var item in items1)
                Console.WriteLine("key : {0}, 아이템 이름 : {1}, 데미지 : {2}",
                    item.Key, item.Value.name, item.Value.damage);

            //items1의 타입 : IEnumerable<KeyValuePair>Select
            //실행 결과
            //key : 0, 아이템 이름 : 장검, 데미지 : 10
            //key : 1, 아이템 이름 : 단검, 데미지 : 8


            //itemList에서 데미지가 12 미만인 아이템 가져오기.
            IEnumerable<Item> items2 = itemList.Where(x => x.damage < 12);
            foreach (var item in items2)
                Console.WriteLine("id : {0}, 아이템 이름 : {1}, 데미지 : {2}",
                    item.id, item.name, item.damage);

            //items2의 타입 : IEnumerable<Item>
            //실행 결과
            //id : 0, 아이템 이름 : 장검, 데미지 : 10
            //id : 1, 아이템 이름 : 단검, 데미지 : 8
        }
    }

    #region
    //try
    //{
    //    RegistryKey key = Registry.CurrentUser.OpenSubKey("DBHitek", true);
    //    if (key == null)
    //    {
    //        Registry.CurrentUser.CreateSubKey("DBHitek", true);
    //        key = Registry.CurrentUser.OpenSubKey("DBHitek", true);
    //    }
    //    key.SetValue("AdminConsoleModePopup", true);
    //}
    //catch (Exception ex)
    //{
    //    MessageBox.Show(ex.Message);
    //}
    #endregion
}