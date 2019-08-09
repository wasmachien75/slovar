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
      <div class="lemma">
        <span>{{lemmaWithStressMarker}}</span>
      </div>
      <div class="definition">
        <p v-bind:key="index" v-for="(item,index) in this.definitionParts">{{item}}</p>
      </div>
    </div>
  </div>
</template>

<script>
import AutoComplete from "vuejs-auto-complete";

export default {
  name: "app",
  data: function() {
    return {
      selectedItem: null,
      definitions: [],
      apiEndpoint:
        (process.env.NODE_ENV === "development"
          ? "https://localhost:5001"
          : "") + "/api/search?startsWith="
    };
  },
  computed: {
    definitionParts: function() {
      if (this.selectedItem != null) {
        return this.formatDescription(this.selectedItem.definition).split("\n");
      }
      return [];
    },
    lemma: function() {
      return this.selectedItem && this.selectedItem.lemma;
    },
    lemmaWithStressMarker: function() {
      return (
        this.lemma &&
        this.combineLemmaWithStressIndicator(
          this.lemma,
          this.selectedItem.stressIndex
        )
      );
    }
  },
  components: {
    AutoComplete
  },
  methods: {
    log(something) {
      console.log(something);
    },
    display(something) {
      this.selectedItem = something.selectedObject;
    },
    clearDescription() {
      this.selectedItem = null;
    },
    formatDescription(stringDescription) {
      //insert newlines before 1., 2.,... if there is none
      return stringDescription.replace(/[^\n\d](?=\d+\.)/, "$&\n");
    },
    combineLemmaWithStressIndicator(string, stressIndex) {
      if (string == null) {
        return "";
      }
      if (stressIndex == null) {
        return string;
      }
      return (
        string.substring(0, stressIndex) +
        String.fromCharCode(769) +
        string.substring(stressIndex)
      );
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

.lemma {
  font-weight: bold;
}

.description {
  p {
    line-height: 1.4em;
  }
}

.autocomplete {
  input {
    font-size: 1.2em;
    padding: 3px;
    font-family: $font;
  }
}
</style>
