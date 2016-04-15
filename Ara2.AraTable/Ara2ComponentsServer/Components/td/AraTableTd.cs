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
    [AraDevComponent(vResizable:false,vDraggable:false)]
    public class AraTableTd : AraComponentVisualConteiner,IAraDev
    {
        public AraTableTd(IAraContainerClient ConteinerFather)
            : base(AraObjectClienteServer.Create(ConteinerFather, "td"), ConteinerFather, "AraTableTd")
        {
            this.TypePosition = ETypePosition.Static;

            Click = new AraComponentEvent<EventHandler>(this, "Click");
            this.EventInternal += AraTableTd_EventInternal;

            if (!(ConteinerFather is AraTableTr))
                throw new Exception("This object can only be added AraTable");
        }

        public override void LoadJS()
        {
            Tick vTick = Tick.GetTick();
            vTick.Session.AddJs("Ara2/Components/AraTable/td/AraTableTd.js");
        }

        private int _colspan = 0;
        [AraDevProperty(0)]
        public int ColSpan
        {
            set
            {
                _colspan = value;
                Tick vTick = Tick.GetTick();
                this.TickScriptCall();
                vTick.Script.Send(" vObj.SetColSpan('" + _colspan.ToString() + "'); \n");
            }
            get { return _colspan; }
        }

        private int _rowspan = 0;
        [AraDevProperty(0)]
        public int RowSpan
        {
            set
            {
                _rowspan = value;
                Tick vTick = Tick.GetTick();
                this.TickScriptCall();
                vTick.Script.Send(" vObj.SetRowSpan('" + _rowspan.ToString() + "'); \n");
            }
            get { return _rowspan; }
        }

        public void AraTableTd_EventInternal(String vFunction)
        {
            switch (vFunction.ToUpper())
            {
                case "CLICK":
                    Click.InvokeEvent(this, new EventArgs());
                    break;
            }
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
