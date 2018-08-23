﻿using System;
using System.Collections.Generic;

namespace ClientApp.ViewModels {
    using System.ComponentModel;
    using System.Reflection;
    using System.Runtime.CompilerServices;

    namespace ViewModels {

        /**
         * Ripped from someone way smarter than me for reference: 
         * https://github.com/johnshew/Minimal-UWP-MVVM-CRUD/blob/master/Simple%20MVVM%20UWP%20with%20CRUD/ViewModels/ViewModelHelpers.cs
         */

        public class NotificationBase : INotifyPropertyChanged {
            public event PropertyChangedEventHandler PropertyChanged;

            // SetField (Name, value); // where there is a data member
            protected bool SetProperty<T>(ref T field, T value, [CallerMemberName] String property
           = null) {
                if (EqualityComparer<T>.Default.Equals(field, value)) return false;
                field = value;
                RaisePropertyChanged(property);
                return true;
            }

            // SetField(()=> somewhere.Name = value; somewhere.Name, value) 
            // Advanced case where you rely on another property
            protected bool SetProperty<T>(T currentValue, T newValue, Action DoSet,
                [CallerMemberName] String property = null) {
                if (EqualityComparer<T>.Default.Equals(currentValue, newValue)) return false;
                DoSet.Invoke();
                RaisePropertyChanged(property);
                return true;
            }

            protected void RaisePropertyChanged(string property) {
                if (PropertyChanged != null) {
                    PropertyChanged(this, new PropertyChangedEventArgs(property));
                }
            }

            public void RaiseAllPropertyChanged() {
                PropertyInfo[] properties = GetType().GetProperties();
                foreach (PropertyInfo prop in properties) {
                    RaisePropertyChanged(prop.Name);
                }
            }
        }

        public class NotificationBase<T> : NotificationBase where T : class, new() {
            protected T This;

            public static implicit operator T(NotificationBase<T> thing) { return thing.This; }

            public NotificationBase(T thing = null) {
                This = (thing == null) ? new T() : thing;
            }
        }
    }
}
