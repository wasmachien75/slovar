export default function() {
  const _size = 2;
  const _cache = {};
  const cache = function() {
    return {
      put: (obj, name) => {
        let cachedKeys = Object.keys(_cache);
        if (cachedKeys.length > _size) {
          let _stale = cachedKeys[0];
          delete cache[_stale];
        }
        _cache[name] = obj;
      },
      get: name => _cache[name],
      _cache: _cache
    };
  };

  return cache;
}
