using System;
using System.Configuration;
using System.IO;
using Free64.Common;

namespace Free64
{
    public static class ConfigControl
    {
        /// <summary>
        /// Minimal slow update time
        /// </summary>
        private const float SlowUpdate = 0.2F;

        /// <summary>
        /// Write <see cref="string"/> in the specified key of configuration
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string Write(string key, string value)
        {
            try
            {
                double Time = CommonThings.GetUNIXTime();

                int DebugMessage = Free64Application.Debug.SendMessage($"[Writing XML] Writing {'"' + key + '"'}...");

                Configuration Conf = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                Conf.AppSettings.Settings.Remove(key);
                Conf.AppSettings.Settings.Add(key, value);
                Conf.Save();

                Time = Math.Round(CommonThings.GetUNIXTime() - Time, 4);
                if (Time == 0) Time = 0.0001;

                if (Time >= SlowUpdate) Free64Application.Debug.AppendToMessage($" (Successful) Slow configuration update. Configuration updated in {Time} sec...", DebugMessage);
                else Free64Application.Debug.AppendToMessage($" Successful... Configuration updated in {Time} sec...", DebugMessage);
            }
            catch (ConfigurationErrorsException e)
            {
                File.Delete(e.Filename);
            }
            catch (Exception e)
            {
                Free64Application.Debug.SendMessage($"[Writing XML] {e.Message}");
            }
            return value;
        }

        /// <summary>
        /// Write <see cref="{T}"/> in the specified key of configuration
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static T Write<T>(string key, T value)
        {
            try
            {
                double Time = CommonThings.GetUNIXTime();

                int DebugMessage = Free64Application.Debug.SendMessage($"[Writing XML] Writing {'"' + key + '"'}...");

                Configuration Conf = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                Conf.AppSettings.Settings.Remove(key);
                Conf.AppSettings.Settings.Add(key, value.ToString());
                Conf.Save();

                Time = Math.Round(CommonThings.GetUNIXTime() - Time, 4);
                if (Time == 0) Time = 0.0001;
               
                if (Time >= SlowUpdate) Free64Application.Debug.AppendToMessage($" (Successful) Slow configuration update. Configuration updated in {Time} sec...", DebugMessage);
                else Free64Application.Debug.AppendToMessage($" Successful... Configuration updated in {Time} sec...", DebugMessage);
            }
            catch (ConfigurationErrorsException e)
            {
                File.Delete(e.Filename);
            }
            catch (Exception e)
            {
                Free64Application.Debug.SendMessage($"[Writing XML] {e.Message}");
            }
            return value;
        }

        /// <summary>
        /// Read <see cref="string"/> from configuration.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string Read(string key)
        {
            return ConfigurationManager.AppSettings.Get(key);
        }
    }
}
