﻿using BusinessLogic.DtoModels;
using CommunityToolkit.Mvvm.Messaging.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPF_UI.Messages
{
    public class BomSelectedMessage : ValueChangedMessage<BomDto>
    {
        public BomSelectedMessage(BomDto value) : base(value)
        {
        }
    }
}
