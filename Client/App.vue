<template>
  <div id="app">
    <AutoComplete
      v-bind:source="this.rootEndPoint + '/api/search?startsWith='"
      results-display="lemma"
      results-property="results"
      placeholder="Введите слово"
      input-class="search"
      @selected="display"
    >
      <template #noResults>Ничего не найдено.</template>
    </AutoComplete>
    <h5 @click="getRandom()">Random</h5>
    <div class="entry" :class="{fetching: this.isFetching}">
      <Lemma :lemma="this.lemma" :stressIndex="this.stressIndex" />
      <Definition :definition="this.definition" v-if="this.definition" />
      <Translation :data="this.translation" v-if="this.translation" />
    </div>
  </div>
</template>

<script>
import AutoComplete from "vuejs-auto-complete";
import Translation from "./Translation.vue";
import Definition from "./Definition.vue";
import Lemma from "./Lemma.vue";

export default {
  name: "app",
  data: function() {
    return {
      cache: null,
      selectedItem: null,
      isFetching: false,
      rootEndPoint:
        process.env.NODE_ENV === "development" ? "http://localhost:5001" : ""
    };
  },
  computed: {
    lemma: function() {
      return this.selectedItem && this.selectedItem.lemma;
    },
    stressIndex: function() {
      return this.selectedItem && this.selectedItem.stressIndex;
    },
    definition: function() {
      return this.selectedItem && this.selectedItem.definition;
    },
    translation: function() {
      return this.selectedItem && this.selectedItem.translation;
    }
  },
  components: {
    AutoComplete,
    Translation,
    Definition,
    Lemma
  },
  watch: {
    $route: "getFromRoute"
  },
  methods: {
    display(something) {
      this.selectedItem = something.selectedObject;
      this.$router.push(`/${this.lemma}`);
    },
    async getRandom() {
      this.selectedItem = await this.fetchJson("/api/random");
      this.$router.push(`/${this.lemma}`);
    },
    async get(lemma) {
      let lemmaObj = this.cache.get(lemma);
      if (!lemmaObj) {
        this.isFetching = true;
        let lemmaObj = await this.fetchJson(`/api/lemma/${lemma}`);
        this.cache.put(lemmaObj, lemma);
      }
      this.selectedItem = lemmaObj;
    },

    async fetchJson(relativeUrl) {
      console.log("Fetching " + relativeUrl);
      this.isFetching = true;
      try {
        let response = await fetch(this.rootEndPoint + relativeUrl);
        if (response.ok) {
          return await response.json();
        } else {
          console.log("Error while fetching " + relativeUrl);
        }
      } catch (e) {
        console.log(e);
      } finally {
        this.isFetching = false;
      }
    },
    async getFromRoute() {
      let lemma = this.$route.params.lemma;
      if (lemma && lemma !== this.lemma) {
        await this.get(lemma);
      }
    },
    initCache() {
      var _cache = {};
      let cache = function() {
        return {
          put: (obj, name) => {
            _cache[name] = obj;
          },
          get: name => _cache[name]
        };
      };

      return cache;
    }
  },
  mounted: async function() {
    this.cache = this.initCache()();
    await this.getFromRoute();
  }
};
</script>

<style lang="scss">
@import url("https://fonts.googleapis.com/css?family=PT+Serif:400,700&display=swap&subset=cyrillic");
$font: "PT Serif", serif;
#app {
  font-family: $font;
  -webkit-font-smoothing: antialiased;
  -moz-osx-font-smoothing: grayscale;
  max-width: 600px;
  margin: 50px auto;
}

h5 {
  font-variant: small-caps;
  cursor: pointer;
}

.entry {
  border-left: solid 5px #aaa;
  padding-left: 16px;
  margin-top: 45px;
  font-family: $font;
}

.autocomplete {
  input {
    font-size: 1.2em;
    padding: 3px;
    font-family: $font;
  }
}
</style>
