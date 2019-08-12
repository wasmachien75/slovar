<template>
  <div id="app">
    <AutoComplete
      v-bind:source="this.apiEndpoint"
      results-display="lemma"
      results-property="results"
      placeholder="Введите слово"
      input-class="search"
      @selected="display"
    >
      <template #noResults>Ничего не найдено.</template>
    </AutoComplete>
    <div class="entry">
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
      selectedItem: null,
      apiEndpoint:
        (process.env.NODE_ENV === "development"
          ? "https://localhost:5001"
          : "") + "/api/search?startsWith="
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
  methods: {
    display(something) {
      this.selectedItem = something.selectedObject;
    }
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
