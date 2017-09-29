using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace projectQ
{
    public class UIClienteleViewModel : UIModelBase
    {
        public UIClienteleView UI
        {
            get { return _ui as UIClienteleView; }
        }

        public List<long> onlinePlayers;
        public List<long> relevancePlayers;

        public List<ClienteleItemData> onlineData;
        public List<ClienteleItemData> relevanceData;

        protected override GEnum.NamedEvent[] FocusNetWorkData()
        {
            return new GEnum.NamedEvent[]
            {
                GEnum.NamedEvent.SysUI_RoomPlayers_Over,
            };
        }

        protected override void OnNetWorkDataCallBack(GEnum.NamedEvent msgEnum, object[] data)
        {
            InitList();

            switch (msgEnum)
            {
                case GEnum.NamedEvent.SysUI_RoomPlayers_Over:

                    onlinePlayers = (List<long>)data[0];
                    for (int i = 0; i < onlinePlayers.Count; i++)
                    {
                        ClienteleItemData clidata = new ClienteleItemData();
                        clidata.player = MemoryData.PlayerData.get(onlinePlayers[i]);
                        onlineData.Add(clidata);
                    }

                    onlineData = SortDataList(onlineData);

                    relevancePlayers = (List<long>)data[1];
                    for (int i = 0; i < relevancePlayers.Count; i++)
                    {
                        ClienteleItemData clidata = new ClienteleItemData();
                        clidata.player = MemoryData.PlayerData.get(relevancePlayers[i]);
                        relevanceData.Add(clidata);
                    }

                    relevanceData = SortDataList(relevanceData);

                    UI.RefreshPlayerData();
                    break;

            }
        }

        private List<ClienteleItemData> SortDataList(List<ClienteleItemData> sortList)
        {
            List<ClienteleItemData> list = new List<ClienteleItemData>();
            List<ClienteleItemData> temporary = new List<ClienteleItemData>();
            List<ClienteleItemData> temporary2 = new List<ClienteleItemData>();
            List<ClienteleItemData> temporary3 = new List<ClienteleItemData>();

            for (int i = 0; i < sortList.Count; i++)
            {
                if (sortList[i].player.ClienteleDataState.ClienteleState == ClienteleDataState.e_ClienteleState.isPlay)
                {
                    temporary.Add(sortList[i]);
                }
                else if (sortList[i].player._clienteleDataState.ClienteleState == ClienteleDataState.e_ClienteleState.onLine)
                {
                    temporary2.Add(sortList[i]);
                }
                else if (sortList[i].player._clienteleDataState.ClienteleState == ClienteleDataState.e_ClienteleState.outLine)
                {
                    temporary3.Add(sortList[i]);
                }
            }

            for (int i = 0; i < temporary.Count - 1; i++)
            {
                for (int n = 1; n < temporary.Count; n++)
                {
                    if (temporary[i].player._clienteleDataState.deskID > temporary[n].player._clienteleDataState.deskID)
                    {
                        ClienteleItemData data = temporary[i];
                        temporary[i] = temporary[n];
                        temporary[n] = data;
                    }
                }
            }

            list.AddRange(temporary);
            list.AddRange(temporary2);
            list.AddRange(temporary3);

            return list;
        }

        private void InitList()
        {
            if (onlineData == null)
                onlineData = new List<ClienteleItemData>();
            else
                onlineData.Clear();

            if (relevanceData == null)
                relevanceData = new List<ClienteleItemData>();
            else
                relevanceData.Clear();
        }
    }
}

