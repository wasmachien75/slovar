<template>
  <section class="translation">
    <h4>Перевод</h4>
    <div class="translation">
      <div :class="getClass(item)" v-bind:key="index" v-for="(item,index) in this.translationParts">
        {{item}}
        <br />
      </div>
    </div>
  </section>
</template>

<script>
export default {
  props: {
    data: String
  },
  methods: {
    getClass(definition) {
      if (!definition.match(/[A-Za-z]/)) {
        return "hidden";
      }
      if (definition.match(/^\s*\d/)) {
        return "li";
      }
      return "";
    }
  },
  computed: {
    translationParts: function() {
      if (this.data) {
        return this.data.split("\n");
      }
    }
  }
};
</script>

<style lang="scss">
div.translation {
  .hidden {
    display: none;
  }
  .li {
    margin-bottom: 8px;
  }
}

h4 {
}
</style>