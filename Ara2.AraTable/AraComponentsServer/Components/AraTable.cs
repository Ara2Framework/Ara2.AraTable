// Copyright (c) 2010-2016, Rafael Leonel Pontani. All rights reserved.
// For licensing, see LICENSE.md or http://www.araframework.com.br/license
// This file is part of AraFramework project details visit http://www.arafrework.com.br
// AraFramework - Rafael Leonel Pontani, 2016-4-14
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using Ara2;
using Ara2.Dev;

namespace Ara2.Components
{
    [Serializable]
    [AraDevComponent(vCompatibleChildrenTypes:new Type[] { typeof(AraTableTr) })]
    public class AraTable : AraComponentVisualAnchorConteiner,IAraDev
    {
        public AraTable(IAraContainerClient ConteinerFather)
            : base(AraObjectClienteServer.Create(ConteinerFather, "table"), ConteinerFather, "AraTable")
        {
            Click = new AraComponentEvent<EventHandler>(this, "Click");
            this.EventInternal += AraTable_EventInternal;

            this.AddChildBefore += AraTable_AddChildBefore;

            this.MinWidth = 10;
            this.MinHeight = 10;
            this.Width = 100;
            this.Height = 100;
        }
        public override void LoadJS()
        {
            Tick vTick = Tick.GetTick();
            vTick.Session.AddJs("Ara2/Components/AraTable/AraTable.js");
        }

        public void AraTable_EventInternal(String vFunction)
        {
            switch (vFunction.ToUpper())
            {
                case "CLICK":
                    Click.InvokeEvent(this, new EventArgs());
                    break;
            }
        }

        private void AraTable_AddChildBefore(IAraObject Child)
        {
            if (!(Child is AraTableTr || Child is AraResizable || Child is AraDraggable))
                throw new Exception("This container is only allowed AraTableTr");
        }

        [AraDevEvent]
        AraComponentEvent<EventHandler> Click;

        #region Ara2Dev
        private string _Name = "";
        [AraDevProperty("")]
        public string Name
        {
            get { return _Name; }
            set { _Name = value; }
        }

        private AraEvent<DStartEditPropertys> _StartEditPropertys = null;
        public AraEvent<DStartEditPropertys> StartEditPropertys
        {
            get
            {
                if (_StartEditPropertys == null)
                {
                    _StartEditPropertys = new AraEvent<DStartEditPropertys>();
                    this.Click += this_ClickEdit;
                }

                return _StartEditPropertys;
            }
            set
            {
                _StartEditPropertys = value;
            }
        }
        private void this_ClickEdit(object sender, EventArgs e)
        {
            if (_StartEditPropertys.InvokeEvent != null)
                _StartEditPropertys.InvokeEvent(this);
        }

        private AraEvent<DStartEditPropertys> _ChangeProperty = new AraEvent<DStartEditPropertys>();
        public AraEvent<DStartEditPropertys> ChangeProperty
        {
            get
            {
                return _ChangeProperty;
            }
            set
            {
                _ChangeProperty = value;
            }
        }



        #endregion

    }
}
