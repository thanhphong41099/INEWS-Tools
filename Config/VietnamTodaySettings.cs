using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;

namespace News2025
{
    /// <summary>
    /// Quản lý danh sách Scene (string->path) và Layer (string->int) đọc từ app.config (section: VietnamTodaySettings).
    /// - Bỏ qua key trống hoặc value rỗng.
    /// - Tự nhận diện key kết thúc bằng "Scene" hoặc "Layer".
    /// - Nếu trùng key: lấy giá trị cuối cùng (ghi đè).
    /// </summary>
    public sealed class VietnamTodaySettings
    {
        public IReadOnlyDictionary<string, string> Scenes => _scenes;
        public IReadOnlyDictionary<string, int> Layers => _layers;

        private readonly Dictionary<string, string> _scenes;
        private readonly Dictionary<string, int> _layers;

        private VietnamTodaySettings(Dictionary<string, string> scenes, Dictionary<string, int> layers)
        {
            _scenes = scenes;
            _layers = layers;
        }

        public static VietnamTodaySettings Load(NameValueCollection section)
        {
            var scenes = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
            var layers = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);

            if (section == null) throw new ArgumentNullException(nameof(section));

            foreach (var rawKey in section.AllKeys)
            {
                if (string.IsNullOrWhiteSpace(rawKey)) continue;

                // Lấy TẤT CẢ value theo key (NameValueCollection có thể có nhiều value cùng key)
                var values = section.GetValues(rawKey);
                if (values == null || values.Length == 0) continue;

                // Lấy value cuối cùng (ghi đè) – tránh trường hợp key "Scene" trống nhiều dòng
                var value = values[values.Length - 1];
                if (string.IsNullOrWhiteSpace(value)) continue;

                if (rawKey.EndsWith("Scene", StringComparison.OrdinalIgnoreCase))
                {
                    // Chuẩn hoá đường dẫn nếu có thể
                    var path = value.Trim();
                    try
                    {
                        // Không bắt buộc file tồn tại, nhưng nếu có thì chuẩn hoá cho đẹp
                        if (File.Exists(path)) path = Path.GetFullPath(path);
                    }
                    catch { /* bỏ qua lỗi path */ }

                    scenes[rawKey] = path;
                }
                else if (rawKey.EndsWith("Layer", StringComparison.OrdinalIgnoreCase))
                {
                    if (int.TryParse(value.Trim(), out var layer))
                        layers[rawKey] = layer;
                }
                else
                {
                    // Bỏ qua các key khác (ví dụ placeholder "Scene" rỗng bạn đã ghi chú)
                }
            }

            return new VietnamTodaySettings(scenes, layers);
        }

        // Truy cập tiện dụng
        public bool TryGetScene(string key, out string path) => _scenes.TryGetValue(key, out path);
        public bool TryGetLayer(string key, out int layer) => _layers.TryGetValue(key, out layer);

        public string GetScene(string key) => _scenes.TryGetValue(key, out var p) ? p : null;
        public int? GetLayer(string key) => _layers.TryGetValue(key, out var l) ? l : (int?)null;

        public string RequireScene(string key)
        {
            return _scenes.TryGetValue(key, out var p) && !string.IsNullOrWhiteSpace(p) ? p : null;
        }

        // Bản "bắt buộc có", throw nếu thiếu
        public int RequireLayer(string key)
        {
            if (!_layers.TryGetValue(key, out var l))
                throw new KeyNotFoundException($"Layer '{key}' chưa được cấu hình.");
            return l;
        }
    }
}
